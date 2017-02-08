using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views.InputMethods;
using System.Linq;
using Passave.DataBaseWork;
using Passave.Activitys;
using Newtonsoft.Json;
using System;
using Android.Content.PM;

namespace Passave
{
    [Activity]
    public class ListPageActivity : Activity
    {
        private TextView EmptyList;
        private ListView mList;
        private List<Info> myInfo;
        private EditText mSearch;
        private LinearLayout mContainer;
        private MyListViewAdapter adapter;
        private DriverDB data;
        private User CurrentUser;
        private bool mAnimatedDown;
        private bool mIsAnimating;

        private int pos;
        private string NewName;

        private string OldPassword;
        private string NewPassword;
        private string ConfirmNewPassword;

        private string NewUrl;
        private string NewLogin;
        private string EditNewPassword;
        private string NewDescription;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ListPage);

            mList = FindViewById<ListView>(Resource.Id.myListView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            EmptyList = FindViewById<TextView>(Resource.Id.textNoItems);

            RegisterForContextMenu(mList);


            CurrentUser = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("User"));

            this.Title = CurrentUser.Name;
            data = new DriverDB();
            myInfo = new List<Info>();

            if (data.SelectAllFromInfo().Count > 0)
            {
                foreach (var item in data.SelectAllFromInfo())
                {
                    if (item.User_id == CurrentUser.ID)
                    {
                        myInfo.Add(item);
                    }
                }
            }
            if(myInfo.Count > 0)
            {
                EmptyList.Visibility = ViewStates.Invisible;
            }

            if (this.Intent.GetStringExtra("Url") != null)
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                AddFragment addEvent = new AddFragment(this.Intent.GetStringExtra("Url"));
                addEvent.Show(transaction, "addEvent");
                addEvent.mAddEventAgrs += AddEvent_mAddEventAgrs;
            }

            adapter = new MyListViewAdapter(this, myInfo);
            mList.Adapter = adapter;

            mList.ItemClick += MList_ItemClick;

            mSearch.Alpha = 0;
            mContainer.BringToFront();
            mSearch.TextChanged += MSearch_TextChanged;
        }

        //Создание Контекстного Меню Для Каждого Элемента Из Списка
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            menu.SetHeaderTitle(adapter[info.Position].Name);
            menu.Add(0, 1, 0, "Редактировать");
            menu.Add(0, 2, 0, "Поделиться");
            menu.Add(0, 3, 0, "Удалить");
        }
        //Обработчик События При Клике На Пункт Контекстного Меню
        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            pos = info.Position;
            switch (item.ItemId)
            {
                case 1:
                    FragmentTransaction tran = FragmentManager.BeginTransaction();
                    EditInfoFragment editFragment = new EditInfoFragment(adapter[pos].Name, adapter[pos].Login, adapter[pos].Password, adapter[pos].Description);
                    editFragment.Show(tran, "EditInfo");
                    editFragment.eventUrl += EditFragment_eventUrl;
                    editFragment.eventLogin += EditFragment_eventLogin;
                    editFragment.eventPassword += EditFragment_eventPassword;
                    editFragment.eventDescription += EditFragment_eventDescription;
                    editFragment.eventConfirm += EditFragment_eventConfirm;
                    break;
                case 2:
                    Intent intent = new Intent(Intent.ActionSend);
                    intent.SetType("text/plain");
                    intent.PutExtra(Intent.ExtraText, adapter[pos].Name + "\r\n\r\n" + adapter[pos].Description);
                    Intent chosenIntent = Intent.CreateChooser(intent, "Поделиться Ссылкой:");
                    StartActivity(chosenIntent);
                    break;
                case 3:
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Confirm delete of:");
                    alert.SetMessage("Вы действительно хотите удалить - " + adapter[pos].Name + " из вашего списка?");
                    alert.SetPositiveButton("Delete", (senderAlert, args) =>
                    {
                        data.DelFromInfo(adapter[pos].ID);
                        myInfo.RemoveAt(pos);
                        adapter = new MyListViewAdapter(this, myInfo);
                        mList.Adapter = adapter;
                        if (myInfo.Count == 0)
                        {
                            EmptyList.Visibility = ViewStates.Visible;
                        }
                    });
                    alert.SetNegativeButton("Cancel", (senderAlert, args) => { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    break;
            }
            return base.OnContextItemSelected(item);
        }
        //Создание Меню В верхней Панеле
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        //Обработчик События При Клике На Пункт Меню
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.save:
                    bool exist = false;
                    var searchQuery = "com.google.android.apps.docs";
                    var flag = PackageInfoFlags.Activities;
                    var apps = PackageManager.GetInstalledApplications(flag);
                    foreach (var app in apps)
                    {
                        if (app.PackageName == searchQuery)
                        {
                            var uri = Android.Net.Uri.Parse(@"https://drive.google.com");
                            Intent intent = new Intent(Intent.ActionSend, uri);
                            intent.SetType("text/plain");
                            string res = "Your Data From Passave";
                            foreach (var inf in myInfo)
                            {
                                res += String.Format($"\n\n{inf.Name}\n{inf.Login}\n{inf.Password}\n{inf.Description}");
                            }
                            intent.PutExtra(Intent.ExtraText, res);
                            StartActivity(intent);
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
                    {
                        var uri = Android.Net.Uri.Parse(@"https://drive.google.com");
                        Intent intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                    }
                    return true;
                case Resource.Id.search:
                    mSearch.Visibility = ViewStates.Visible;
                    if (mIsAnimating)
                    {
                        return true;
                    }

                    if (!mAnimatedDown)
                    {
                        //Listview is up
                        MyAnimation anim = new MyAnimation(mList, mList.Height - mSearch.Height);
                        anim.Duration = 500;
                        mList.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
                    }
                    else
                    {
                        //Listview is down
                        MyAnimation anim = new MyAnimation(mList, mList.Height + mSearch.Height);
                        anim.Duration = 500;
                        mList.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartUp;
                        anim.AnimationEnd += anim_AnimationEndUp;
                        mContainer.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
                    }
                    mAnimatedDown = !mAnimatedDown;
                    return true;
                case Resource.Id.add:
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    AddFragment addEvent = new AddFragment();
                    addEvent.Show(transaction, "addEvend");
                    addEvent.mAddEventAgrs += AddEvent_mAddEventAgrs;
                    return true;
                case Resource.Id.ChangeName:
                    FragmentTransaction transaction2 = FragmentManager.BeginTransaction();
                    ChangeNameFragment changeName = new ChangeNameFragment(CurrentUser.Name);
                    changeName.Show(transaction2, "changeName");
                    changeName.mEditName += ChangeName_mEditName;
                    changeName.mGetEditText += ChangeName_mGetEditText;
                    return true;
                case Resource.Id.ChangePassword:
                    FragmentTransaction transaction3 = FragmentManager.BeginTransaction();
                    ChangePasswordFragment changePassword = new ChangePasswordFragment();
                    changePassword.Show(transaction3, "changePassword");
                    changePassword.eventOldPassword += ChangePassword_eventOldPassword;
                    changePassword.eventNewPassword += ChangePassword_eventNewPassword;
                    changePassword.eventConfirmNewPassword += ChangePassword_eventConfirmNewPassword;
                    changePassword.eventConfirm += ChangePassword_eventConfirm;
                    return true;
                case Resource.Id.LogOut:
                    StartActivity(typeof(MainActivity));
                    this.Finish();
                    return true;
                case Resource.Id.Support:
                    FragmentTransaction transaction4 = FragmentManager.BeginTransaction();
                    SupportFragment support = new SupportFragment();
                    support.Show(transaction4, "support");
                    return true;
                case Resource.Id.About:
                    StartActivity(typeof(AboutActivity));
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        //Методы Редактирования Информации О Сайте
        private void EditFragment_eventDescription(object sender, EditText e)
        {
             NewDescription = e.Text;
        }
        private void EditFragment_eventPassword(object sender, EditText e)
        {
             EditNewPassword = e.Text;
        }
        private void EditFragment_eventLogin(object sender, EditText e)
        {
             NewLogin = e.Text;
        }
        private void EditFragment_eventUrl(object sender, EditText e)
        {
             NewUrl = e.Text;
        }
        private void EditFragment_eventConfirm(object sender, Button e)
        {
            if (NewUrl == null)
                NewUrl = adapter[pos].Name;
            if (NewLogin == null)
                NewLogin = adapter[pos].Login;
            if (EditNewPassword == null)
                EditNewPassword = adapter[pos].Password;
            if (NewDescription == null)
                NewDescription = adapter[pos].Description;

            data.UpdateInfo(adapter[pos].ID, NewUrl, NewLogin, EditNewPassword, NewDescription);

            adapter[pos].Name = NewUrl;
            adapter[pos].Login = NewLogin;
            adapter[pos].Password = EditNewPassword;
            adapter[pos].Description = NewDescription;

            mList.Adapter = adapter;

            NewUrl = null;
            NewLogin = null;
            EditNewPassword = null;
            NewDescription = null;
        }
        

        //Метод Поиска Сайта Из Общего Списка
        private void MSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            List<Info> searchedInfo = (from info in myInfo where info.Name.Contains(mSearch.Text) select info).ToList<Info>();
            if(searchedInfo.Count > 0)
            {
                adapter = new MyListViewAdapter(this, searchedInfo);
                mList.Adapter = adapter;
            }
        }

        //Метод Вывода Информации О Сайте При Клике На Сайт Из Списка
        private void MList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            InformationFragment info = new InformationFragment(adapter[e.Position].Name, adapter[e.Position].Login, adapter[e.Position].Password, adapter[e.Position].Description);
            info.Show(transaction, "InfoShow");
        }

        //Методы Изменения Пароля учетной Записи
        private void ChangePassword_eventConfirm(object sender, Button e)
        {
            if(OldPassword == CurrentUser.Password)
            {
                if(NewPassword == null)
                {
                    Toast.MakeText(this, "Вы Не Ввели Новый Пароль", ToastLength.Short).Show();
                }
                else
                {
                    if(NewPassword == ConfirmNewPassword)
                    {
                        data.UpdateUser(CurrentUser.ID, this.Title, NewPassword);
                        CurrentUser.Password = NewPassword;
                        Toast.MakeText(this, "Вы Успешно Изменили Свой Пароль!", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Вы Не Подтвердили Новый Пароль!", ToastLength.Short).Show();
                    }
                }
            }
            else
            {
                Toast.MakeText(this, "Вы ввели неправильный старый пароль!", ToastLength.Short).Show();
            }
        }
        private void ChangePassword_eventConfirmNewPassword(object sender, EditText e)
        {
            ConfirmNewPassword = e.Text;
        }
        private void ChangePassword_eventNewPassword(object sender, EditText e)
        {
            NewPassword = e.Text;
        }
        private void ChangePassword_eventOldPassword(object sender, EditText e)
        {
            OldPassword = e.Text;
        }

        //Методы для изменения имени пользователя
        private void ChangeName_mGetEditText(object sender, EditText e)
        {
            NewName = e.Text;
        }
        private void ChangeName_mEditName(object sender, Button e)
        {
            this.Title = NewName;
            data.UpdateUser(CurrentUser.ID, NewName, CurrentUser.Password);
            CurrentUser.Name = NewName;
        }


        //Метод Добавления Нового Сайта В Список
        private void AddEvent_mAddEventAgrs(object sender, Info e)
        {
            if(e.Name == "")
            {
                Toast.MakeText(this, "Вы Не Ввели Имя Сайта", ToastLength.Short).Show();
            }
            else
            {
                data.InsertIntoInfo(e.Name, e.Login, e.Password, e.Description, CurrentUser.ID);
                myInfo.Clear();
                if (data.SelectAllFromInfo().Count > 0)
                {
                    foreach (var item in data.SelectAllFromInfo())
                    {
                        if (item.User_id == CurrentUser.ID)
                        {
                            myInfo.Add(item);
                        }
                    }
                }
                adapter = new MyListViewAdapter(this, myInfo);
                mList.Adapter = adapter;
                EmptyList.Visibility = ViewStates.Invisible;
            }
        }


        //Методы Анимации Поля Поиска
        void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
            mSearch.ClearFocus();
            mSearch.Visibility = ViewStates.Invisible;
        }
        void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
        }
        void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(ShowFlags.Implicit, 0);
        }
        void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(-1.0f).SetDuration(500).Start();
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }
}

