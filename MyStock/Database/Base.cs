using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Base : IBase
    {
        /// <summary>
        /// Variável que contém o endereço de conexão com o banco de dados
        /// </summary>
        private string ConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

        /// <summary>
        /// Converte o tipo da propriedade C# para a nomenclatura SQL
        /// </summary>
        public string TipoPropriedade(PropertyInfo pi)
        {
            switch (pi.PropertyType.Name)
            {
                case "Int32":
                    return "INT";
                case "Int64":
                    return "BIGINT";
                case "Double":
                    return "NUMERIC(12,2)";
                case "Single":
                    return "FLOAT";
                case "DateTime":
                    return "DATETIME";
                case "Boolean":
                    return "TINYINT";
                default:
                    return "VARCHAR(255)";
            }
        }

        /// <summary>
        /// Método genérico para salvar no Banco de Dados 
        /// </summary>
        public virtual void Salvar(int acao)
        {
            /// Utiliza uma string contendo o endereço do servidor, o nome do banco e o acesso do SQL Server 
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                List<string> valores = new List<string>();

                /// Retorna o atributo da classe 
                foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    /// Retorna as propriedades do atributo 
                    OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));

                    if (pOpcoesBase != null && pOpcoesBase.UsarNoBancoDeDados)
                    {
                        /// Se for Double troca-se as vírgulas por pontos e retira-se os pontos marcadores de casas 
                        if (pi.PropertyType.Name == "Decimal")
                        {
                            valores.Add(" " + pi.GetValue(this).ToString().Replace(".", "").Replace(",", "."));
                        }
                        else
                        {
                            /// Pega o valor que foi inserido no atributo do objeto e o armazena em uma lista 
                            valores.Add("'" + pi.GetValue(this) + "'");
                        }
                    }
                }

                string queryString = string.Empty;


                // concatenar a variável ação

                /// Inserir 
                if (acao == 1)
                {
                    /// Concatena a instrução de execução da procedure com os valores da lista
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  1, " + string.Join(", ", valores.ToArray()) + ";";
                }
                /// Editar 
                else if (acao == 2)
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  2, " + string.Join(", ", valores.ToArray()) + ";";
                }
                /// Excluir
                else if (acao == 3)
                {
                    queryString = "EXEC uspGerir" + this.GetType().Name + "  3, " + string.Join(", ", valores.ToArray()) + ";";
                }

                /// Abre a conexão com banco e executa a query 
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Método utilizado para capturar todos os dados de uma tabela sql e exibi-la na interface
        /// </summary>
        public virtual List<IBase> Todos()
        {
            var list = new List<IBase>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT * FROM " + this.GetType().Name;
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                /// Cria uma variável com o comando de leitura
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    /// Cria a instância do objeto com base na sua classe 
                    var obj = (IBase)Activator.CreateInstance(this.GetType());
                    setProperty(ref obj, reader);
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        /// Executa a ação de uma procedure que verifica a existência de um código
        /// enviado por parâmetro 
        /// </summary>
        /// <param name="codigoBuscar"> Valor da chave primária do produto </param>
        /// <returns></returns>
        public virtual List<IBase> Busca(string codigoBuscar)
        {
            var list = new List<IBase>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "EXEC uspBuscarCodigo" + this.GetType().Name + " '" + codigoBuscar + "';";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var obj = (IBase)Activator.CreateInstance(this.GetType());
                    setProperty(ref obj, reader);
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        /// Método utilizado para setar o select no SQL dentro de uma lista 
        /// </summary>
        /// <param name="obj"> Um objeto referenciado, ou seja, todas as alterações feitas aqui são refletidas na variável pai </param>
        /// <param name="reader"> Variável que contém a linha SQL </param>
        private void setProperty(ref IBase obj, SqlDataReader reader)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                OpcoesBase pOpcoesBase = (OpcoesBase)pi.GetCustomAttribute(typeof(OpcoesBase));
                if (pOpcoesBase != null && pOpcoesBase.UsarNoBancoDeDados)
                {
                    /// Seta o valor contido no índice [pi.Name] para o atributo do objeto
                    pi.SetValue(obj, reader[pi.Name]);
                }
            }
        }
    }
}
