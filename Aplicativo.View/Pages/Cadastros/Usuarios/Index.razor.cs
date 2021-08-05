using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using ExpressionPowerTools.Core.Dependencies;
using ExpressionPowerTools.Core.Extensions;
using ExpressionPowerTools.Serialization;
using ExpressionPowerTools.Serialization.Compression;
using ExpressionPowerTools.Serialization.EFCore.Http.Extensions;
using ExpressionPowerTools.Serialization.EFCore.Http.Queryable;
using ExpressionPowerTools.Serialization.Signatures;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Aplicativo.View.Pages.Cadastros.Usuarios
{
    public partial class IndexPage : ComponentBase
    {

        protected ListItemViewLayout<Usuario> ListView { get; set; }
        protected ViewUsuario View { get; set; }

        protected async Task Page_Load()
        {
            await BtnPesquisar_Click();
        }

        protected async Task BtnPesquisar_Click()
        {

            //var query = DbClientContext<Context>.Query(c => c.Usuario);

            //query = query.Where(c => c.UsuarioID == 32);

            //var Usuarios = await query.ExecuteRemote().ToListAsync();


            var Query = new HelpQuery<Usuario>();

            Query.AddWhere("Ativo == @0", true);

            ListView.Items = await Query.ToList();
            
        }

        protected async Task BtnItemView_Click(object args)
        {
            await View.EditItemViewLayout.Show(args);
        }

        protected async Task BtnExcluir_Click(object args)
        {
            await View.Excluir(((IEnumerable)args).Cast<Usuario>().Select(c => (int)c.UsuarioID).ToList());
        }


    }
}