using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<string> animeList = new List<string>();
    static List<double> animeTotalTimes = new List<double>();

    static void Main()
    {
        do
        {
            RunAnimeProgram();
            Console.Write("Do you want to use the program again? (Y)es or (N)o: ");
            Console.WriteLine(" ");
        } while (Console.ReadLine()?.ToLower() == "y");

        ListAllAnimeEntered();
    }

    static void RunAnimeProgram()
    {
        Console.WriteLine(" ");
        Console.Write("What is the name of the anime? ");
        string animeName = Console.ReadLine();
        animeList.Add(animeName);

        int episodeNumber = GetIntegerInput("How many episodes are there? ");
        double episodeDuration = GetDoubleInput("How long is the average episode in minutes? ");
        Console.WriteLine(" ");

        double time = Math.Round((episodeNumber * episodeDuration) / 60.0, 2);
        animeTotalTimes.Add(time);

        Console.Write("Are there any sequels or spinoffs you want to include? (Y)es or (N)o ");
        string sequelYes = Console.ReadLine();
        Console.WriteLine(" ");

        List<string> sequelTitles = new List<string>();
        List<int> sequelEpisodeList = new List<int>();
        List<double> sequelTimeList = new List<double>();

        if (sequelYes.ToLower() == "y")
        {
            int sequelNumber = GetIntegerInput("How many sequels or spinoffs are there? ");
            Console.WriteLine(" ");

            while (sequelNumber > 0)
            {
                string sequelTitle = GetStringInput("What is the name of the sequel? ");
                sequelTitles.Add(sequelTitle);

                int sequelEpisodeNumber = GetIntegerInput("How many episodes are there? ");
                sequelEpisodeList.Add(sequelEpisodeNumber);

                double sequelEpisodeDuration = GetDoubleInput("How long is the average episode in minutes? ");
                double sequelTime = Math.Round((sequelEpisodeNumber * sequelEpisodeDuration) / 60.0, 2);
                sequelTimeList.Add(sequelTime);
                Console.WriteLine(" ");

                sequelNumber--;
            }

            int episodeNumberTotal = sequelEpisodeList.Sum() + episodeNumber;
            double totalTime = Math.Round(sequelTimeList.Sum() + time, 2);

            Console.WriteLine($"There are a total of {episodeNumberTotal} episodes");
            Console.WriteLine($"This anime, including all its sequels and spinoffs, will take {totalTime} hours to complete");
            Console.WriteLine(" ");
            animeTotalTimes[animeTotalTimes.Count - 1] = totalTime; // Update the total time for the main anime entry
        }
        else if (sequelYes.ToLower() == "n")
        {
            Console.WriteLine($"This anime will take {time} hours to complete");
            Console.WriteLine(" ");
        }

        string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string oneDriveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive");

        string folderPath = Path.Combine(Directory.Exists(documentsFolder) ? documentsFolder : oneDriveFolder, "Anime Calculations");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, $"{animeName}.txt");

        using (StreamWriter sourceFile = new StreamWriter(filePath))
        {
            sourceFile.WriteLine(animeName);
            if (sequelYes.ToLower() == "y")
            {
                sourceFile.WriteLine($"Including sequels or spinoffs, there are {sequelEpisodeList.Sum() + episodeNumber} episodes for this anime.");
                sourceFile.WriteLine($"This anime, including all its sequels and spinoffs, will take {sequelTimeList.Sum() + time} hours to complete");

                sourceFile.WriteLine("\nIndividual Entries:");
                sourceFile.WriteLine($"{animeName} - {episodeNumber} episodes, {time} hours");

                for (int i = 0; i < sequelTitles.Count; i++)
                {
                    sourceFile.WriteLine($"{sequelTitles[i]} - {sequelEpisodeList[i]} episodes, {sequelTimeList[i]} hours");
                }
            }
            else if (sequelYes.ToLower() == "n")
            {
                sourceFile.WriteLine($"This anime has {episodeNumber} episodes.");
                sourceFile.WriteLine($"This anime will take {time} hours to complete.");
            }
        }
    }

    static void ListAllAnimeEntered()
    {
        Console.WriteLine("List of all anime entered (sorted by highest to lowest total time):");

        var sortedAnimeList = animeList
            .Select((anime, index) => new { Name = anime, Time = animeTotalTimes[index] })
            .OrderByDescending(a => a.Time);

        foreach (var animeEntry in sortedAnimeList)
        {
            Console.WriteLine($"{animeEntry.Name} - {animeEntry.Time} hours");
        }
    }

    static int GetIntegerInput(string prompt)
    {
        int result;
        do
        {
            Console.Write(prompt);
        } while (!int.TryParse(Console.ReadLine(), out result));
        return result;
    }

    static double GetDoubleInput(string prompt)
    {
        double result;
        do
        {
            Console.Write(prompt);
        } while (!double.TryParse(Console.ReadLine(), out result));
        return result;
    }

    static string GetStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
