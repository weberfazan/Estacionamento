using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;//Imports para conexão do Banco de Dados
using MySql.Data.MySqlClient;//Imports para realizar comandos no banco de dados

namespace Estacionamento
{
    class DaoFuncionario
    {
        MySqlConnection conexao;
        public string dados;
        public string resultado;
        public int[] cod;
        public string[] nome;
        public long[] CPF;
        public DateTime[] dataNascimento;
        public string[] telefone;
        public string[] funcao;
        public int i;
        public int contadorLinhasTabela;
        public double[] salario;
        public string[] endereco;
        public int opcao;

        //Construtor
        public DaoFuncionario()
        {
            conexao = new MySqlConnection("server=localhost;DataBase=Estacionamentoti13n;Uid=root;Password=;Convert Zero Datetime=True");
            try
            {
                conexao.Open();//solicitando a entrada do banco de dados                
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!!!\n\n" + e);
                conexao.Close();//fechando a conexão com o danco de dados
            }//fim da tentativa de conexão com o banco de dados
        }//fim do metodo construtor

        public void MenuFuncionario()
        {
            Console.WriteLine("\n\nEscolha uma das opções abaixo: \n\n" +
            "\n1. Cadastrar" +
            "\n2. Consultar Tudo" +
            "\n3. Consultar Individual" +
            "\n4. Atualizar" +
            "\n5. Excluir");
            opcao = Convert.ToInt32(Console.ReadLine());
        }

        public void ExecutarFuncionario()
        {

            do
            {
                MenuFuncionario();

                switch (opcao)
                {
                    case 1:
                        //Colentando os dados
                        Console.WriteLine("Informe seu nome: ");
                        string nome = Console.ReadLine();
                        Console.WriteLine("\nInforme seu telefone: ");
                        string telefone = Console.ReadLine();
                        Console.WriteLine("\nInforme DataDeNascimento: ");
                        DateTime DataDeNascimento = Convert.ToDateTime(Console.ReadLine());
                        Console.WriteLine("Informe o seu CPF:");
                        long CPF = (long)Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Informe funcao");
                        string funcao = Console.ReadLine();
                        Console.WriteLine("Informe salario");
                        double salario = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("\nInforme o endereço:");
                        string endereco = Console.ReadLine();

                        //Executar o método inserir
                        Inserir( nome, telefone, DataDeNascimento,salario,  CPF, funcao, endereco);

                        break;

                    case 2:

                        ConsultarTudo();

                        break;

                    case 3:

                        Console.WriteLine("\nInsira o codigo que você quer consultar:");

                        int codigo= Convert.ToInt32(Console.Read());

                        ConsultarIndividual(codigo);


                        break;

                    case 4:


                        Console.WriteLine("\n\nDigite o codigo do Funcionario");
                        codigo= Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Digite o campo que quer alterar:");
                        string campo = Console.ReadLine();
                        Console.WriteLine("Digite o novo dado");
                        string novoDado = Console.ReadLine();

                        Atualizar(campo, novoDado, codigo);
                            




                        break;

                    case 5:

                        Console.WriteLine("Insira o codigo do Funcionario que você quer excluir");
                        codigo = Convert.ToInt32(Console.ReadLine());

                        Deletar(codigo);

                        break;

                    default:

                        Console.WriteLine("Digite um valor válido!");

                        break;
                }

            } while (opcao != 0);
        }

        //criar o metodo inserir
        public void Inserir(string nome, string telefone, DateTime dataNascimento, double salario, long CPF, string funcao, string endereco)
        {
            try
            {
                dados = $"('', '{nome}', '{telefone}', '{CPF}', '{endereco}', '{dataNascimento:yyyy-MM-dd}', '{funcao}', '{salario}')";
                resultado = "Insert into Funcionario(codigo, nome, telefone, CPF, endereco, dataDeNascimento, funcao, salario) values" + dados;
                //Executar o comando resultado no banco de dados
                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine(resultado + "Linha(s) Afetadas!");

            }//fim do try

            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!\n\n" + e);

            }//fim do catch

        }//fim do metodo Inserir

        public void PreencherVetor()
        {
            string query = "select * from funcionario"; //coletando dado do Banco de Dados

            //Instanciando os Vetores
            cod = new int[100];
            nome = new string[100];
            telefone = new string[100];
            CPF = new long[100];
            funcao = new string[100];
            dataNascimento = new DateTime[100];
            salario = new double[100];
            endereco = new string[100];

            //Dar valores iniciais para ele
            for (i = 0; i < 100; i++)
            {
                cod[i] = 0;
                nome[i] = "";
                telefone[i] = "";
                dataNascimento[i] = new DateTime();
                salario[i] = 0;
                CPF[i] = 0;
                funcao[i] = "";
                endereco[i] = "";
            }//fim da repetição

            //criar o comando para coletar dados
            MySqlCommand coletar = new MySqlCommand(query, conexao);
            //usar os comandos lendo os dados do banco de dados
            MySqlDataReader leitura = coletar.ExecuteReader();

            i = 0;
            while (leitura.Read())
            {
                cod[i] = Convert.ToInt32(leitura["codigo"] + "");
                nome[i] = leitura["nome"] + "";
                telefone[i] = leitura["telefone"] + "";
                CPF[i] = Convert.ToInt64(leitura ["CPF"] + "");
                dataNascimento[i] = Convert.ToDateTime(leitura["dataDeNascimento"] + "");
                salario[i] = Convert.ToDouble(leitura["valorPagamento"] + "");
                funcao[i] = leitura["funcao"] + "";
                endereco[i] = leitura["endereco"] + "";
                i++;
                contadorLinhasTabela++;
            }//fim do metodo while

            //fechar o dataReader
            leitura.Close();

        }//fim do preencher vetor

        public void ConsultarIndividual(int codigo)
        {
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {

                if (codigo == cod[i])
                {
                    Console.WriteLine("\n\n Codigo:" + cod[i] +
                                      ",Nome: " + nome[i] +
                                      ",Telefone: " + telefone[i] +
                                      ",Data de Nascimento: " + dataNascimento[i].ToString("dd/MM/yyyy") +
                                      ",salario: " + salario[i] +
                                      ",CPF: " + CPF[i] +
                                      "Função: " + funcao[i] +
                                      "Endereço: " + endereco[i]);
                }

            }

            Console.WriteLine("Código não encontrado!");
        }

        public void ConsultarTudo()
        {
            //Preencher o Vetor
            PreencherVetor();

            for (int i = 0; i < contadorLinhasTabela; i++)
            {
                Console.WriteLine("\n\n Codigo: " + cod[i]
                                      + ",Nome: " + nome[i]
                                      + ",Telefone: " + telefone[i]
                                      + ",Data de Nascimento: " + dataNascimento[i].ToString("dd/MM/yyyy")
                                      + ",salario: " + salario[i]
                                      + ",CPF: " + CPF[i]
                                      + ",Função: " + funcao[i]);
            }//fim do for

        }//fim do consultar tudo

        public void Atualizar(string campo, string novoDado, int codigo)
        {
            try
            {
                resultado = "update funcionario set" + campo + " = '" +
                             novoDado + "' where codigo = '" + codigo + "'";
                //Executar o script
                MySqlCommand sql = new MySqlCommand(resultado, conexao);
                resultado = "" + sql.ExecuteNonQuery();
                Console.WriteLine("Dado Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo deu errado!" + e);

            }


        }//fim do Atualizar

        public void Deletar(int codigo)
        {
            resultado = "delete from funcionario where codigo = '" + codigo + "'";
            //Executar o Comando
            MySqlCommand sql = new MySqlCommand(resultado, conexao);
            resultado = "" + sql.ExecuteNonQuery();
            //Mensagem...
            Console.WriteLine("Dados Excluido com sucesso!");
        }//fim do deletar


    }//fim da Classe Cliente

}//fim do projeto
