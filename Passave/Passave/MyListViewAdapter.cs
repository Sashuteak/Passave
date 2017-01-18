using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Net;

namespace Passave
{
    class MyListViewAdapter : BaseAdapter<Info>
    {
        private List<Info> mitems;
        private Context mContext;

        public MyListViewAdapter(Context context, List<Info> items)
        {
            mContext = context;
            mitems = items;
        }
        public override Info this[int position]
        {
            get
            {
                return mitems[position];
            }
        }
        public override int Count
        {
            get
            {
                return mitems.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);
            }
            TextView mText = row.FindViewById<TextView>(Resource.Id.txtName);

            if(mitems[position].Name.Contains("http") || mitems[position].Name.Contains("https"))
            {
                Uri uri = Uri.Parse(mitems[position].Name);
                mText.Text = uri.Host;
            }
            else
            {
                mText.Text = mitems[position].Name;
            }
            return row;
        }
    }
}