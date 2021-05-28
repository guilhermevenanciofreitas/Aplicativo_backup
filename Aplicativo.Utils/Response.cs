using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicativo.Utils
{

    public enum StatusCode
    {

        //Requisição realizada com sucesso
        Success = 100,

        //Validação de campo
        Validation = 101,

        //Sem autorização para requisição
        NotAutorized = 150,


        //Usuário desconectado
        LoginRequired = 200,

        //Erro não tratado
        Error = 400,

    }

    public class Response
    {

        public StatusCode StatusCode { get; set; } = StatusCode.Success;

        public object Data { get; set; }

    }
}
