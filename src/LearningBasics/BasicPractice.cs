namespace BeginCsh.LearningBasics;
static class BasicPractice
{
    /*static void Main()
    {
            //string Greeting = "General Transporty!";
           /*string? Saying = Console.ReadLine();

           if (Saying == "Hello there!")
           {
               Console.WriteLine(Greeting);
           }   else
           {
               Console.WriteLine("You are not welcome here!");
           }*/

           /*Console.WriteLine($"Ваш ответ содержит {Saying?.Length} символов.");
           Console.WriteLine($"Ваш ответ в верхнем регистре: {Saying?.ToUpper()}");
           Console.WriteLine($"Ваш ответ в нижнем регистре: {Saying?.ToLower()}");*/

           /*Greeting = "Hi my name is AHHHH, Im 13 y.o.";
            Console.WriteLine(Greeting.StartsWith("Hi") ? "Greeting starts with 'Hi'" : "Greeting does not start with 'Hi'");
            Console.WriteLine(Greeting.EndsWith("y.o.") ? "Greeting ends with 'y.o.'" : "Greeting does not end with 'y.o.'");
            Console.WriteLine(Greeting.EndsWith("o"));
            Console.WriteLine(Greeting.EndsWith("."));*/

            //TODO: ДАЛЬШЕ БУДУ ИЗУЧАТЬ УРОК ПРО ЧИСЛА. КАК РАЗ ПОСЛЕ НЕГО СДЕЛАЮ ПЕРВОЕ ПРАКТИЧЕСКОЕ ОТ ДЖЕМА

            /*void BasicMath()
        {
            int a = 10;
            int b = 3;
            int result = a + b;
            Console.WriteLine(result);

            result = a / b;
            int remainder = a % b;
            Console.WriteLine($"Division result: {result}, Remainder: {remainder}");
        }

        Console.WriteLine("Basic Math Operations:");
        BasicMath();*/

        /*void minmax()
        {
             int maxValue = int.MaxValue;
            int minValue = int.MinValue;
            Console.WriteLine($"Max Value: {maxValue}, Min Value: {minValue}");
        }*/
    
        /*DecimalSupremacy();
        void DecimalSupremacy()
        {
            double aD = 1;
            double bD = 3;
            decimal aDec = 1;
            decimal bDec = 3;
            double resultD = aD / bD;
            decimal resultDec = aDec / bDec;

            Console.WriteLine($"Double Division Result: {resultD}");
            Console.WriteLine($"Decimal Division Result: {resultDec}");
            
            decimal a1D = 1.0M;
            decimal b1D = 3.0M;
            decimal result1Dec = a1D / b1D;
            Console.WriteLine($"Decimal Division Result (with M suffix): {result1Dec}");
    }  */

        /*RadiusSearch();
        void RadiusSearch()
        {
            double radius = 2.50;
            double area = Math.PI*radius*radius;
            Console.WriteLine($"Area of circle with radius {radius} is: {area}");
        }*/

        /*VarTesting();
        void VarTesting()
        {
            var mixed = (a: 1, b: 2.5, c: "Hello");
            var mixedgoods = (mixed.a * mixed.b);
            Console.WriteLine($"Mixed goods: {mixedgoods}");

            var mixed2 = mixed with { c = "World" };
            Console.WriteLine($"Mixed2: {mixed2}");
        }*/

        /*whileLoop(0);
        void whileLoop(int counter)
        {
            while (counter <= 5)
            {
                Console.WriteLine($"Counter: {counter}");
                counter++;
            }
        }

        forLoop(5);
        void forLoop(int forcounter)
        {
            for (int i = forcounter; i <= 125; i *= 5)
            {
                Console.WriteLine($"For Counter: {i}");
            }
        }*/

        /*CalculateSum();
        void CalculateSum()
        {
            int sum = 0;
            for (int number = 1; number <= 20; number++)
            {
                if (number % 3 == 0)
                {
                    sum += number;
                }
               
            }
            Console.WriteLine($"Sum of multiples of 3 from 1 to 20: {sum}");
        }*/

        //TODO: Закончил немало уроков. Теперь дочитать и попрактиковать уроки коллекции и шаблоны. После этого буду делать практическое задание от Джема.

        /*ListPractice();
        void ListPractice()
        {
           List<int> dates = [1002, 2000, 3500, 40100, 200, 228, 300, 400, 902, 700, 90, 900, 205];
            dates.Sort();
            foreach (var date in dates)
            {
                Console.WriteLine(date);
            }

            List<int> fibonacci = [1, 1];
            
            void GenerateFibonacci(int count)
            {
                for (int i = 2; i < count; i++)
                {
                    var previous = fibonacci[fibonacci.Count - 1];
                    var previous2 = fibonacci[fibonacci.Count - 2];

                    fibonacci.Add(previous + previous2);
                }
            }

            GenerateFibonacci(20);
            foreach (var number in fibonacci)
            {
                Console.WriteLine(number);
            }

            var index = dates.IndexOf(500);
            if (index != -1)
            {
                Console.WriteLine($"Found 500 at index: {index}");
            }
            else
            {
                Console.WriteLine("500 not found in the list.");
            }
            index = dates.IndexOf(200);
            if (index != -1)
            {
                Console.WriteLine($"Found 200 at index: {index}");
            }
            else
            {
                Console.WriteLine("200 not found in the list.");
            }



            //Console.WriteLine($"Total number of dates: {dates.Count}");
            //Console.WriteLine($"{dates[2]}, {dates[1]}");
            
            List<string> names = ["popa", "pipa", "mama", "baba"];
            foreach (var name in names)
            {
                Console.WriteLine($"Hi, name is {name.ToUpper()}");
            }
        }*/

        //TODO закончил основы. Теперь стоит пробежаться по углублению, особенно про создание классов, неймспейсы. 
        // Попутно начать делать практику.
    }    
