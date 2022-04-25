using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Estacionamento
{
    class Menu
    {
        DaoCliente daoCliente;
        DaoFuncionario daoFuncionario;
        DaoMensalista daoMensalista;
        DaoVeiculo daoVeiculo;
        public int opcao;

        public Menu()
        {
            opcao = 0;
            daoCliente = new DaoCliente();
            daoFuncionario = new DaoFuncionario();
            daoMensalista = new DaoMensalista();
            daoVeiculo = new DaoVeiculo();



        }//fim do construtor



        public void MostrarOpcoes()
        {
            Console.WriteLine("\n\n Escolha o que você quer fazer:" +
                "\n1.Cliente" +
                "\n2.Mensalista" +
                "\n3.Funcionário." +
                "\n0. Sair.");
            opcao = Convert.ToInt32(Console.ReadLine());
        }//fim do método



        public void Executar()
        {
            do
            {
                MostrarOpcoes();

                switch (opcao)
                {
                    case 1:
                        daoCliente.ExecutarCliente();

                        
                        break;

                    case 2:

                        daoMensalista.ExecutarMensalista();

                        break;
                    case 3:
                        daoFuncionario.ExecutarFuncionario();

                        

                        break;

                        
                    case 0:
                        Console.WriteLine("Obrigado!");
                        break;
                    default:
                        Console.WriteLine("Código digitado não é valido!");
                        break;
                }//fim do switch_Case
            } while (opcao != 0);
        }//fim do método

    }// Fim da classe

 }//fim do projeto

