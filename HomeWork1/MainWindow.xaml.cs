using System;


namespace HomeWork1
{
  
   class Program
  {    
      static void Main(string[] args)
     {
            string studentName;
            Console.WriteLine("Введите имя студента");
            studentName = Console.ReadLine();
            Console.WriteLine("Имя:" + studentName );

            Console.WriteLine("Решение квадратного уравнения.");
            {
                string A, B, C;
                float D;
                double X1, X2;

                Console.WriteLine("Введите значение A");
                A = Console.ReadLine();

                Console.WriteLine("Введите значение B");
                B = Console.ReadLine();

                Console.WriteLine("Введите значение C");
                C = Console.ReadLine();

                int a, b, c;

                a = Convert.ToInt32(A);
                b = Convert.ToInt32(B);
                c = Convert.ToInt32(C);

                D = b * b - 4 * a * c;
                Console.WriteLine(" Дискриминант = " + D);

                if (D > 0)
                {
                    X1 = (-b + Math.Sqrt(D)) / (2 * a);
                    X2 = (-b - Math.Sqrt(D)) / (2 * a);
                    Console.WriteLine(" Корни дискриминанта: X1 = " + X1);
                    Console.WriteLine(" Корни дискриминанта: X2 = " + X2);
                }

                if (D < 0)
                {
                    Console.WriteLine(" Дискриминант < 0. Корней нет.");
                }

                if (D == 0)
                {
                    X1 = (-b / (2 * a));
                    Console.WriteLine(" Дискриминант =" + X1);
                }

            }

            Console.WriteLine(" Нахождениее гипотенузы по двум катетам.");
            {
                string A, B;
                int a, b,q;
                double c;
                Console.WriteLine(" Введите длину катета а.");
                A = Console.ReadLine();

                Console.WriteLine(" Введите длину катета b.");
                B = Console.ReadLine();

                a = Convert.ToInt32(A);
                b = Convert.ToInt32(B);

                c = Math.Sqrt(a * a + b * b);

                q = Convert.ToInt32(c);
                Console.WriteLine(" Длина гипотенузы =" + q );

                Console.WriteLine("Нахождеение углов треульника.");
                int anglA = 90, anglB, anglC;
                double anglb;
               
       

                anglb = Math.Asin(a/c)/Math.PI *180;
                anglB = Convert.ToInt32(anglb);
                anglC = 180 - anglA - anglB;

                Console.WriteLine("Угол А = "+anglA);
                Console.WriteLine("Угол B = "+anglB);              
                Console.WriteLine("Угол C = "+anglC);
                

                Console.ReadKey();

            }

        }

   }

}

