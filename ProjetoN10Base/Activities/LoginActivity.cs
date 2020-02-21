using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using ProjetoN10Base.Classes;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Preferences;
using System;
using Newtonsoft.Json;

namespace ProjetoN10Base.Activities
{
    [Activity()]
    public class LoginActivity : AppCompatActivity
    {
        EditText edtEmail, edtSenha;
        Button btnEntrar;
        CoordinatorLayout rootViewLogin;

        //Variáveis usadas para manter o login
        ISharedPreferences prefs;
        private bool _dadosMantidos = false;
        private string _emailMantido, _senhaMantida;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        
            //Setando qual layout este activity vai controlar
            SetContentView(Resource.Layout.activity_login);

            //Capturando os componentes do layout que são usados
            rootViewLogin = FindViewById<CoordinatorLayout>(Resource.Id.rootViewLogin);
            edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            edtSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            btnEntrar = FindViewById<Button>(Resource.Id.btnEntrar);

            //Trazer as informações dos dados de login mantidos
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            _dadosMantidos = prefs.GetBoolean("dadosMantidos", false);

            btnEntrar.Click += BtnEntrar_Click;

            //Se o retorno de _dadosMantidos for true
            //quer dizer que o usuário resolveu persistir o login
            if (_dadosMantidos)
            {
                //Recupero as informações armazenadas e clico no botão
                _emailMantido = prefs.GetString("email", "");
                _senhaMantida = prefs.GetString("senha", "");
                btnEntrar.PerformClick();
            }

        }

        private void BtnEntrar_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_dadosMantidos)
                {
                    Pessoa pessoaLogada = new Pessoa().RealizarLogin(_emailMantido, _senhaMantida);
                    Intent tlPrincipal = new Intent(this, typeof(PrincipalActivity));
                    tlPrincipal.PutExtra("pessoaLogada", JsonConvert.SerializeObject(pessoaLogada));
                    tlPrincipal.PutExtra("dadosMantidos", _dadosMantidos);
                    StartActivityForResult(tlPrincipal, 1);
                }
                else
                {
                    Pessoa pessoaLogada = new Pessoa().RealizarLogin(edtEmail.Text, edtSenha.Text);
                    Intent tlPrincipal = new Intent(this, typeof(PrincipalActivity));
                    tlPrincipal.PutExtra("pessoaLogada", JsonConvert.SerializeObject(pessoaLogada));
                    tlPrincipal.PutExtra("dadosMantidos", _dadosMantidos);
                    StartActivityForResult(tlPrincipal, 1);
                }
                
            }
            catch (Exception ex)
            {
                Snackbar.Make(rootViewLogin, ex.Message + ". Limpar campos?", Snackbar.LengthLong).SetAction("Aceitar", delegate {
                    edtEmail.Text = string.Empty;
                    edtSenha.Text = string.Empty;
                    edtEmail.RequestFocus();
                }).Show();
            }
            
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok && data != null)
            {
                _dadosMantidos = data.GetBooleanExtra("dadosMantidos", true);
                if (!_dadosMantidos)
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutBoolean("dadosMantidos", false);
                    editor.PutString("email", string.Empty);
                    editor.PutString("senha", string.Empty);
                    editor.Apply();

                    edtEmail.Text = string.Empty;
                    edtSenha.Text = string.Empty;
                    edtEmail.RequestFocus();
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}