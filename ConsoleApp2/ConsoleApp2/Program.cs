using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Phone
    {
        public string Name { get; set; }
        public string Company { get; set; }

    }
    class Player
    {
        public string Name { get; set; }
        public int Team_id { get; set; }
    }
    class Team
    {
        public int Id { get; set; }
        public string Country { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Grouping
            List<Phone> phones = new List<Phone> {
                new Phone{Name = "Lumia 430", Company = "Microsoft"},
                new Phone{Name = "Mi 5", Company = "Xiaomi"},
                new Phone{Name = "LG G 3", Company = "LG"},
                new Phone{Name = "iPhone 5s", Company = "Apple"},
                new Phone{Name = "Lumia 930", Company = "Microsoft"},
                new Phone{Name = "iPhone 6s", Company = "Apple"},
                new Phone{Name = "LG G 6", Company = "LG"},
                new Phone{Name = "Lumia 430", Company = "Microsoft"},
                new Phone{Name = "Sony xz 5", Company = "Sony"},
            };
            List<Team> teams = new List<Team> {
                new Team { Id =1, Country="Германия"}, 
                new Team { Id = 2, Country="Испания"}
            };

            List<Player> players = new List<Player> {
                new Player{Name="Месси", Team_id=2},
                new Player{Name="Неймар", Team_id=2},
                new Player{Name="Роббен", Team_id=1},
            };

            GroupingExample(phones);
            InnerQuery(phones);
            JoinExample(teams, players);
            LazyExample(new String[] { "Бавария", "Боруссия", "Реал Мадрид", "Манчестер Сити", "ПСЖ", "Барселоноа"});
            Console.ReadKey();

        }

        private static void LazyExample(string[] teams)
        {
            //отложеный запрос
            Console.WriteLine("--------------------");
            IEnumerable<string> i = from t in teams
                                    where t.ToUpper().StartsWith("Б")
                                    orderby t
                                    select t;
            Console.WriteLine(i.Count());
            teams[1] = "Ювентус";
            Console.WriteLine(i.Count());

            //Немедленный запрос
            Console.WriteLine("--------------------");
            int count = (from t in teams
                    where t.ToUpper().StartsWith("Б")
                    orderby t
                    select t).Count();
            Console.WriteLine(count);
            teams[1] = "Ювентус";
            Console.WriteLine(count);

            Console.WriteLine("----------------");
            var selectedTeams = teams.Where(t => t.ToUpper().StartsWith("Б")).OrderBy(t => t).ToList();

            teams[1] = "Боруссия";

            foreach (var item in selectedTeams)
            {
                Console.WriteLine(item);
            }


        }

        private static void JoinExample(List<Team> teams, List<Player> players)
        {
            var result = from player in players
                         join team in teams on player.Team_id equals team.Id
                         select new { Name = player.Name, Country = team.Country};
            result = players.Join( 
                teams,      //второй набор
                p =>p.Team_id, //свойство обьекта из первого набора
                t=>t.Id, // свойство обьекта из второго набора
                (p, t) => new { Name = p.Name, Country = t.Country } //рузультат
                );
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name}, {item.Country}");
            }   
        }
        private static void InnerQuery(List<Phone> phones)
        {
            //запрос
            var phoneGroups2 = from phone in phones
                               group phone by phone.Company into g
                               select new
                               {
                                   Name = g.Key,
                                   Count = g.Count(),
                                   Phones = from p in g select p
                               };
            //методы
            phoneGroups2 = phones.GroupBy(p => p.Company)
                .Select(g => new {
                    Name = g.Key,
                    Count = g.Count(),
                    Phones = g.Select(p=>p)
                });


            foreach (var group in phoneGroups2)
            {
                Console.WriteLine($"{group.Name}: {group.Count}");
                foreach (Phone phone in group.Phones)
                {
                    Console.WriteLine($"{phone.Name}");
                }
                Console.WriteLine("---------------------");
            }
        }
        private static void GroupingExample(List<Phone> phones)
        {

            //запрос
            var phoneGroups = from phone in phones
                              group phone by phone.Company;
            //методы
            phoneGroups = phones.GroupBy(p => p.Company);
            //результаты
            foreach (IGrouping<string, Phone> g in phoneGroups)
            {
                Console.WriteLine(g.Key);
                foreach (var t in g)
                {
                    Console.WriteLine(t.Name);
                }
                Console.WriteLine();
            }

            //получение количества 
            //запрос
            var phoneGroups2 = from phone in phones
                               group phone by phone.Company into g
                               select new { Name = g.Key, Count = g.Count()};
            //методы
            phoneGroups2 = phones.GroupBy(p => p.Company)
                .Select(g => new { Name = g.Key, Count = g.Count() });
            //результат
            foreach (var item in phoneGroups2)
            {
                Console.WriteLine($"{item.Name}: {item.Count} - {item.Count.GetType().Name}");
            }
        }
    }
}
