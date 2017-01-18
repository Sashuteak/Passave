using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace Passave.Activitys
{
    class ChangeNameFragment : DialogFragment
    {
        private TextView mOldName;
        private EditText mNewName;
        private Button mEdit;
        private Button mCancel;
        private string OldName;
        public event EventHandler<Button> mEditName;
        public event EventHandler<EditText> mGetEditText;

        public ChangeNameFragment(string OldName)
        {
            this.OldName = OldName;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ChangeName, container, false);

            mOldName = view.FindViewById<TextView>(Resource.Id.textOldName);
            mNewName = view.FindViewById<EditText>(Resource.Id.editEditName);
            mEdit = view.FindViewById<Button>(Resource.Id.btnEdit);
            mCancel = view.FindViewById<Button>(Resource.Id.btnEditCancel);

            mOldName.Text = OldName;
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            mEdit.Click += MEdit_Click;
            mCancel.Click += MCancel_Click;
            mNewName.AfterTextChanged += MNewName_AfterTextChanged;

            return view;
        }
        private void MNewName_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            mGetEditText.Invoke(this, mNewName);
        }
        private void MCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }
        private void MEdit_Click(object sender, EventArgs e)
        {
            mEditName.Invoke(this, mEdit);
            this.Dismiss();
        }
    }
}