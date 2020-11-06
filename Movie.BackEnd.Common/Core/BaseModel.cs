using System;

namespace Movie.BackEnd.Common.Core
{
    [Serializable]
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            SetUnchanged();
        }
        private ModelState state;
        public ModelState State
        {
            get { return state; }
            set { state = value; }
        }

        public Boolean IsAdded
        {
            get { return state.Equals(ModelState.Added); }
        }
        public Boolean IsModified
        {
            get { return state.Equals(ModelState.Modified); }
        }
        public Boolean IsDeleted
        {
            get { return state.Equals(ModelState.Deleted); }
        }
        public Boolean IsUnchanged
        {
            get { return state.Equals(ModelState.Unchanged); }
        }
        public Boolean IsDetached
        {
            get { return state.Equals(ModelState.Detached); }
        }
        public void SetAdded()
        {
            state = ModelState.Added;
        }
        public void SetModified()
        {
            state = ModelState.Modified;
        }
        public void SetDeleted()
        {
            state = IsAdded ? ModelState.Detached : ModelState.Deleted;
        }
        public void SetDetached()
        {
            state = ModelState.Detached;
        }
        public void SetUnchanged()
        {
            state = ModelState.Unchanged;
        }

    }
}
