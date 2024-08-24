using Android.Content;
using Android.Runtime;

namespace MegaCaller
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const int RequestCallPermissionId = 1;


        private EditText? EditText {  get; set; }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


           EditText = FindViewById<EditText>(Resource.Id.editTextPhone);

           var buttonCall = FindViewById(Resource.Id.buttonCall);


            buttonCall.Click += (sender, e) =>
            {
                MakePhoneCall(EditText.Text ?? "");
                // Acción que se ejecuta cuando el botón es clicado
                Toast.MakeText(this, "Botón clickeado"+ EditText.Text, ToastLength.Short).Show();
            };
        }

        private void MakePhoneCall(string phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (CheckSelfPermission(Android.Manifest.Permission.CallPhone) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new string[] { Android.Manifest.Permission.CallPhone }, RequestCallPermissionId);
                }
                else
                {
                    StartCall(phoneNumber);
                }
            }
            else
            {
                Toast.MakeText(this, "Please enter a phone number", ToastLength.Short).Show();
            }
        }

        private void StartCall(string phoneNumber)
        {
            var dial = "tel:" + phoneNumber;

            var dialinfo = Android.Net.Uri.Parse(dial);
            Intent intent = new(Intent.ActionCall, dialinfo);
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            if (requestCode == RequestCallPermissionId && grantResults.Length > 0 && grantResults[0] == Android.Content.PM.Permission.Granted)
            {
                var phoneNumber = FindViewById<EditText>(Resource.Id.editTextPhone).Text;
                StartCall(phoneNumber);
            }
            else
            {
                Toast.MakeText(this, "Call permission is required to make a call", ToastLength.Short).Show();
            }
        }


    }
}