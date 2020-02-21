using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using ProjetoN10Base.Classes;
using static Android.Support.Design.Widget.NavigationView;
using ToolbarV7 = Android.Support.V7.Widget.Toolbar;

namespace ProjetoN10Base.Activities
{
    [Activity()]
    public class PrincipalActivity : AppCompatActivity, IOnNavigationItemSelectedListener
    {
        CoordinatorLayout rootViewPrincipal;
        DrawerLayout drawerPrincipal;
        ToolbarV7 tlbPrincipal;
        NavigationView navigationView;

        List<Pessoa> listaPessoas;
        Pessoa pessoaLogada;

        bool dadosMantidos;
        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_principal);

            rootViewPrincipal = FindViewById<CoordinatorLayout>(Resource.Id.rootViewPrincipal);
            drawerPrincipal = FindViewById<DrawerLayout>(Resource.Id.drawerPrincipal);
            tlbPrincipal = FindViewById<ToolbarV7>(Resource.Id.tlbPrincipal);
            navigationView = FindViewById<NavigationView>(Resource.Id.navigationView);

            SetSupportActionBar(tlbPrincipal);
            SupportActionBar.Title = "Lista";

            //Adiciona o menu hamburguer na toolbar, além de ser "tocável" e ter sprint
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerPrincipal, tlbPrincipal, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawerPrincipal.AddDrawerListener(toggle);
            toggle.SyncState();

            //Pegar o usuário logado da tela de Login
            pessoaLogada = JsonConvert.DeserializeObject<Pessoa>(Intent.GetStringExtra("pessoaLogada"));

            //Faz com que possamos clicar nos itens do navigationView
            //(que está dentro do drawerLayout). Precisa implementar uma interface*
            navigationView.SetNavigationItemSelectedListener(this);

            //Pegar os dados de login do usuário caso já esteja mantido
            dadosMantidos = Intent.GetBooleanExtra("dadosMantidos", false);

            //Perguntar pro usuário se ele quer manter os dados de login
            //Somente se estiver falso
            if (!dadosMantidos)
            {
                Snackbar.Make(rootViewPrincipal, "Deseja logar-se automaticamente?", Snackbar.LengthLong).SetAction("Aceitar", delegate
                {
                    prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutBoolean("dadosMantidos", true);
                    editor.PutString("email", pessoaLogada.Email);
                    editor.PutString("senha", pessoaLogada.Senha);
                    editor.Apply();
                }).Show();
            }

            
        }

        //Este método controla as ações dos itens do menu do navigationView
        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_sair:

                    Intent logout = new Intent();
                    logout.PutExtra("dadosMantidos", false);
                    SetResult(Result.Ok, logout);
                    Finish();
                    break;

                default:
                    break;
            }
            return true;
        }

    }
}