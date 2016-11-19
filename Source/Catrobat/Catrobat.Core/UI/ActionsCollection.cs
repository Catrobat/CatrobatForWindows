﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Models.Bricks;
using Catrobat.IDE.Core.Models.Scripts;

namespace Catrobat.IDE.Core.UI
{
    public class ActionsCollection : IList, INotifyCollectionChanged
    {
        public ObservableCollection<Script> Scripts
        {
            get
            {
                if (_selectedSprite == null)
                {
                    return new ObservableCollection<Script>();
                }

                return _selectedSprite.Scripts;
            }
        }
        private Sprite _selectedSprite;
        private Brick _lastDeletedBrick;
        private Brick _lastInsertedBrick;
        private int _lastInsertedIndex;

        public int LastDeletedIndex { get; private set; }

        public ModelBase PreventIsertOfNext { get; set; }

        public void Update(Sprite selectedSprite)
        {
            _selectedSprite = selectedSprite;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddScriptBrick(ModelBase scriptBrick, int firstViewIndex, int lastViewIndex)
        {
            //if (this.Count == lastViewIndex + 1 && GetAtIndex(lastViewIndex) is Script && )
            //{
            //  lastViewIndex++;
            //}

            if (scriptBrick is Brick) // Add brick at last visible end of a Script
            {
                var brick = scriptBrick as Brick;

                var scriptEndIndex = -1;
                Script lastFullScript = null;
                foreach (var script in Scripts)
                {
                    var scriptBeginIndex = scriptEndIndex + 1;
                    scriptEndIndex += script.Bricks.Count + 1;

                    // what does that do?
                    //if (scriptEndIndex > lastViewIndex && scriptBeginIndex >= firstViewIndex)
                    //{
                    //    break;
                    //}

                    lastFullScript = script;
                }

                if (lastFullScript == null)
                {
                    var startScript = new StartScript();
                    Scripts.Add(startScript);
                    lastFullScript = startScript;

                    OnScriptAdded(startScript, IndexOf(startScript));
                }

                lastFullScript.Bricks.Add(brick);

                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); // TODO: make faster and use method below instead
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, scriptBrick, IndexOf(scriptBrick)));
            }
            else if (scriptBrick is Script) // Add Script at end of all
            {
                var script = scriptBrick as Script;
                Scripts.Add(script);

                //OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); // TODO: make faster and use method below instead
                OnScriptAdded((Script) scriptBrick, IndexOf(scriptBrick));
            }
        }

        private void InternalCollectionChanged(object sernder, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.OldItems[0] is Script)
                    {
                        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset); // TODO: change this
                        OnCollectionChanged(args);
                    }
                    else
                    {
                        var args = new NotifyCollectionChangedEventArgs(e.Action, _lastDeletedBrick, LastDeletedIndex);
                        OnCollectionChanged(args);
                    }
                }
                else
                {
                    if (e.NewItems[0] is Script)
                    {
                        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset); // TODO: change this
                        OnCollectionChanged(args);
                    }
                    else
                    {
                        var args = new NotifyCollectionChangedEventArgs(e.Action, _lastInsertedBrick, _lastInsertedIndex);
                        OnCollectionChanged(args);
                    }
                }
            }
            else
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanged(args);
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, e);
            }
        }

        private void OnScriptAdded(Script script, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, script, index));

            var brickIndex = index;
            foreach (var brick in script.Bricks)
            {
                brickIndex++;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, brick, brickIndex));
            }
        }

        private void OnScriptRemoved(Script script, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, script, index));

            var brickIndex = index;
            foreach (var brick in script.Bricks)
            {
                //brickIndex++;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, brick, brickIndex));
            }
        }

        public void RemoveAt(int index)
        {
            Script scriptToRemove = null;
            Brick brickToRemove = null;

            var count = 0;
            foreach (var script in Scripts)
            {
                if (count == index)
                {
                    scriptToRemove = script;
                    break;
                }

                count++;
                foreach (var brick in script.Bricks)
                {
                    if (count == index)
                    {
                        scriptToRemove = script;
                        brickToRemove = brick;

                        _lastDeletedBrick = brick;
                        LastDeletedIndex = index;

                        break;
                    }

                    count++;
                }

                if (brickToRemove != null)
                {
                    break;
                }
            }

            if (brickToRemove == null)
            {
                Scripts.Remove(scriptToRemove);

                OnScriptRemoved(scriptToRemove, index);
            }
            else
            {
                scriptToRemove.Bricks.Remove(brickToRemove);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, brickToRemove, index));
            }
        }

        public void Clear()
        {
            Scripts.Clear();
        }

        public int Count
        {
            get
            {
                return Scripts.Sum(script => script.Bricks.Count + 1) + 1;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ModelBase item)
        {
            if (item != null)
            {
                Remove((object) item);
                return true;
            }

            return false;
        }

        public IEnumerator<ModelBase> GetEnumerator()
        {
            return new ScriptBrickIterator(Scripts);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(object value)
        {
            if (value is Script)
            {
                Scripts.Add((Script) value);
            }

            if (value is Brick)
            {
                Scripts[Scripts.Count - 1].Bricks.Add((Brick) value);
            }

            return 1; // TODO: should probably not be 1 ?
        }

        public bool Contains(object value)
        {
            if (Scripts.Contains(value as Script))
            {
                return true;
            }
            else
            {
                foreach (var script in Scripts)
                {
                    if (script.Bricks.Contains(value as Brick))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int ScriptIndexOf(Script value)
        {
            return Scripts.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            var enumerator = GetEnumerator();

            var count = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == value)
                {
                    return count;
                }

                count++;
            }

            return -1;
        }

        public void Insert(int index, object value)
        {
            if (index == Count)
                index--;

            if (PreventIsertOfNext != null && PreventIsertOfNext == value)
            {
                PreventIsertOfNext = null;
                return;
            }

            var count = 0;

            if (value is Script) // TODO: test me
            {
                var scriptIndex = 0;

                foreach (var script in Scripts)
                {
                    if (count > index)
                    {
                        break;
                    }

                    count += script.Bricks.Count + 1;
                    scriptIndex++;
                }

                Scripts.Insert(scriptIndex, (Script) value);
                OnScriptAdded((Script) value, count + 1);
            }

            if (value is Brick)
            {
                var brickCount = 0;
                _lastInsertedBrick = (Brick) value;
                _lastInsertedIndex = index;

                if (index == 0) // Cannot insert brick before first sprite
                {
                    index = 1;
                }

                foreach (var script in Scripts)
                {
                    count++;
                    foreach (var brick in script.Bricks)
                    {
                        if (count == index)
                        {
                            script.Bricks.Insert(brickCount, (Brick) value);
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
                            return;
                        }

                        count++;
                        brickCount++;
                    }

                    if (count == index)
                    {
                        script.Bricks.Insert(brickCount, (Brick) value);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
                        return;
                    }

                    brickCount = 0;
                }
            }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            var index = IndexOf(value);

            if (value is Script)
            {
                var script = value as Script;
                if (Scripts.Contains(script))
                {
                    Scripts.Remove(script);

                    OnScriptRemoved(script, index);
                }
            }
            else if (value is Brick)
            {
                foreach (var script in Scripts)
                {
                    if (script.Bricks.Contains(value as Brick))
                    {
                        script.Bricks.Remove(value as Brick);
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index));
                    }
                }
            }
        }

        public object this[int index]
        {
            get { return GetAtIndex(index); }
            set { throw new NotImplementedException(); }
        }

        private object GetAtIndex(int index)
        {
            var enumerator = GetEnumerator(); // TODO: make faster do not use enumerator

            var count = 0;
            while (enumerator.MoveNext())
            {
                if (count == index)
                {
                    return enumerator.Current;
                }

                count++;
            }

            return null;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            // TODO: synchroize me
            get { return false; }
        }

        public object SyncRoot
        {
            get { return Scripts; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}