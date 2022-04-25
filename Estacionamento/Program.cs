using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estacionamento
{
    class Program
    {
        static void Main(string[] args)
        {

            Menu menu = new Menu();

            try
            {

            menu.Executar();

            Console.ReadLine();

            }
            catch (Exception e)
            {

                Console.WriteLine($"Algo deu errado!\n\n{e}");

            }


        }//fim do metodo
}//fim da classe
}//fim do projeto
