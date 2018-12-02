// Copyright Olli Etuaho 2018.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LPUnityUtils
{
	[Serializable]
	public class HistoryStateConfig
	{
		[SerializeField] public bool includePosition = true;
		[SerializeField] public bool includeRotation = true;
		[SerializeField] public bool includeParent = true;
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	class UndoHistoryable : Attribute {}

	interface UndoAnimatable
	{
		// Returns true when finished.
		bool AnimateUndo(UndoHistorian.HistoryStateAccess previousState);
	}

	/// <summary>
	/// Historian for saving undo state for example for a puzzle game's undo feature, or for doing changes to game state and then rolling them back.
	/// Can save and restore.
	/// Not very lightweight since it uses reflection to get all the properties to store in history.
	/// It stores all properties annotated with "UndoHistoryable"
    /// 
    /// To use this, add UndoHistorian as a component to all objects you want to store history data for. To save a state to history, call Commit(). To Undo and return to the previous state, call Undo().
    /// 
	/// </summary>
	public class UndoHistorian : MonoBehaviour
	{
		public struct HistoryTransformData
		{
			public Transform parent;
			public Vector3 position;
			public Quaternion rotation;
		}

		public class HistoryState
		{
			// Map from component instance ids to field and property values. The string key is FieldInfo.Name / PropertyInfo.name
			public Dictionary<int, Dictionary<string, object>> storedFields = new Dictionary<int, Dictionary<string, object>>();
			// Map from gameObject instance ids to transform data.
			public Dictionary<int, HistoryTransformData> transformData = new Dictionary<int, HistoryTransformData>();
			public object metadata;
		}

		public class HistoryStateAccess
		{
			public HistoryStateAccess(HistoryState aState, Component aComponent)
			{
				this.state = aState;
				this.component = aComponent;
			}

			private HistoryState state;
			private Component component;

			public object metadata { get { return state.metadata; } }
			public HistoryTransformData transformData { get { return state.transformData[component.gameObject.GetInstanceID()]; } }

			public object StoredField(string fieldName)
			{
				return state.storedFields[component.GetInstanceID()][fieldName];
			}
		}

		private List<HistoryState> history = new List<HistoryState>();

		[SerializeField] HistoryStateConfig config;

		public int Count { get { return history.Count; } }

		public bool animationFinished { get; private set; }

        // Save the state of one object into history.
		public void Commit(object metadata)
		{
			HistoryState state = new HistoryState();
			state.metadata = metadata;
			CommitOne(state, this.gameObject);
			this.history.Add(state);
		}

        // Save the state of one object plus a bunch of other extra objects into history.
		// When restoring this committed state, the same extras in the same order must be passed in.
		public void Commit<T>(object metadata, IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			HistoryState state = new HistoryState();
			state.metadata = metadata;
			CommitOne(state, this.gameObject);
			if (extras != null)
			{
				foreach (T obj in extras)
				{
					CommitOne(state, obj.gameObject);
				}
			}
			this.history.Add(state);
		}

		public void Restore(int index)
		{
			this.Restore<MonoBehaviour>(this.history[index], null);
		}

		public void Restore<T>(int index, IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			this.Restore<T>(this.history[index], extras);
		}

		public void Pop()
		{
			this.history.RemoveAt(this.history.Count - 1);
		}

		public void Undo()
		{
			if (this.history.Count > 1)
			{
				this.history.RemoveAt(this.history.Count - 1);
				this.Restore<MonoBehaviour>(this.history[this.history.Count - 1], null);
			}
		}

		public void Undo<T>(IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			if (this.history.Count > 1)
			{
				this.history.RemoveAt(this.history.Count - 1);
				this.Restore<T>(this.history[this.history.Count - 1], extras);
			}
		}

		/// <summary>
		/// AnimateUndo will do callbacks to components that implement UndoAnimatable.
		/// While the undo animation is playing, the animationFinished field on UndoHistorian is set to false.
		/// Once the undo animation is finished the previous state will also be completely restored,
		/// so it will result in undone state even if the animation is only partially implemented.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void AnimateUndo()
		{
			this.AnimateUndo<MonoBehaviour>(null);
		}

		public void AnimateUndo<T>(IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			if (this.history.Count > 1)
			{
				this.history.RemoveAt(this.history.Count - 1);
				this.StartCoroutine(ProcessAnimateUndo<T>(extras));
			}
		}

		private IEnumerator ProcessAnimateUndo<T>(IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			HistoryState state = this.history[this.history.Count - 1];
			this.animationFinished = false;
			bool finished = false;
			while (!finished) {
				finished = true;
				if (!AnimateOne(state, this.gameObject))
				{
					finished = false;
				}
				if (extras != null)
				{
					foreach (T obj in extras)
					{
						if (!AnimateOne(state, obj.gameObject))
						{
							finished = false;
						}
					}
				}
				this.animationFinished = finished;
				yield return null;
			}
			this.Restore(state, extras);
		}

		// Returns true when animation is finished.
		private bool AnimateOne(HistoryState state, GameObject obj)
		{
			UndoAnimatable[] components = obj.GetComponents<UndoAnimatable>();
			bool finished = true;
			foreach (UndoAnimatable comp in components)
			{
				HistoryStateAccess access = new HistoryStateAccess(state, comp as Component);
				if (!comp.AnimateUndo(access))
				{
					finished = false;
				}
			}
			return finished;
		}

		public bool Reset()
		{
			if (this.history.Count >= 1)
			{
				this.Restore<MonoBehaviour>(this.history[0], null);
				this.Clear();
				return true;
			}
			return false;
		}

		public bool Reset<T>(IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			if (this.history.Count >= 1)
			{
				this.Restore<T>(this.history[0], extras);
				this.Clear();
				return true;
			}
			return false;
		}

		public void Clear()
		{
			this.history.Clear();
		}

		public Transform Parent(int index)
		{
			return this.history[index].transformData[0].parent;
		}

		public Vector3 Position(int index)
		{
			return this.history[index].transformData[0].position;
		}

		public Quaternion Rotation(int index)
		{
			return this.history[index].transformData[0].rotation;
		}

		public object Metadata(int index)
		{
			return this.history[index].metadata;
		}

		private void Restore<T>(HistoryState state, IEnumerable<T> extras)
			where T : MonoBehaviour
		{
			RestoreOne(state, this.gameObject);
			if (extras != null)
			{
				foreach (T obj in extras)
				{
					RestoreOne(state, obj.gameObject);
				}
			}
		}

		private void CommitOne(HistoryState state, GameObject obj)
		{
			Component[] components = obj.GetComponents(typeof(Component));
            UndoHistorian thisHistorian = null;
			foreach (Component comp in components)
			{
				if (comp.GetType() == typeof(UndoHistorian))
				{
                    thisHistorian = comp as UndoHistorian;
                    continue;
				}
				foreach (FieldInfo f in comp.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) )
				{
					foreach (Attribute a in f.GetCustomAttributes(false))
					{
						if (a is UndoHistoryable)
						{
							if (!state.storedFields.ContainsKey(comp.GetInstanceID())) {
								state.storedFields[comp.GetInstanceID()] = new Dictionary<string, object>();
							}
							state.storedFields[comp.GetInstanceID()][f.Name] = f.GetValue(comp);
						}
					}
				}
                foreach (PropertyInfo p in comp.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) )
                {
                    foreach (Attribute a in p.GetCustomAttributes(false))
                    {
                        if (a is UndoHistoryable)
                        {
                            if ( !state.storedFields.ContainsKey(comp.GetInstanceID()) )
                            {
                                state.storedFields[comp.GetInstanceID()] = new Dictionary<string, object>();
                            }
                            state.storedFields[comp.GetInstanceID()][p.Name] = p.GetValue(comp);
                        }
                    }
                }
			}

            if ( thisHistorian != null )
            {
                HistoryTransformData transformDataState = new HistoryTransformData();

                if ( thisHistorian.config.includeParent )
                {
                    transformDataState.parent = obj.transform.parent;
                }
                if ( thisHistorian.config.includePosition )
                {
                    transformDataState.position = obj.transform.position;
                }
                if ( thisHistorian.config.includeRotation )
                {
                    transformDataState.rotation = obj.transform.rotation;
                }
                state.transformData[obj.GetInstanceID()] = transformDataState;
            }
		}

		private void RestoreOne(HistoryState state, GameObject obj)
		{
			Component[] components = obj.GetComponents(typeof(Component));
            UndoHistorian thisHistorian = null;
            foreach (Component comp in components)
			{
                if ( comp.GetType() == typeof(UndoHistorian) )
                {
                    thisHistorian = comp as UndoHistorian;
                    continue;
                }
                foreach (FieldInfo f in comp.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) )
				{
					foreach (Attribute a in f.GetCustomAttributes(false))
					{
						if (a is UndoHistoryable)
						{
							f.SetValue(comp, state.storedFields[comp.GetInstanceID()][f.Name]);
						}
					}
				}
                foreach ( PropertyInfo p in comp.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) )
                {
                    foreach ( Attribute a in p.GetCustomAttributes(false) )
                    {
                        if ( a is UndoHistoryable )
                        {
                            p.SetValue(comp, state.storedFields[comp.GetInstanceID()][p.Name]);
                        }
                    }
                }
            }
            if ( thisHistorian != null )
            {
                if ( thisHistorian.config.includeParent )
                {
                    obj.transform.SetParent(state.transformData[obj.GetInstanceID()].parent, true);
                }
                if ( thisHistorian.config.includePosition )
                {
                    obj.transform.position = state.transformData[obj.GetInstanceID()].position;
                }
                if ( thisHistorian.config.includeRotation )
                {
                    obj.transform.rotation = state.transformData[obj.GetInstanceID()].rotation;
                }
            }
		}
	}

}  // namespace LPUnityUtils
