/*
 * VNTS FORMATTER FOR WORDPRESS POSTS
 * TAKE VNTS TEXT FROM 4CH AND COPY INTO FILE CALLED vnts.txt
 * RUN THIS AND IT MAKES FORMATTED VNTS IN vnts-formatted.txt
 * NEEDS remove.txt, header.txt IN SAME DIRECTORY TO WORK
 * IT SUCKS AND HAS NO ERROR HANDLING SO FAR THANKS BYE
 */

using System.Text.RegularExpressions;

string opener = "Welcome to the Fuwanovel VNTS, where we shamelessly copy " +
    "the work of one illustrious 4chan /jp/ member in collating translation " +
    "progress and add minor dabs of value through corrections and additions. " +
    "Entries with updates this week in <b>bold</b>.\n\n<h3>Fan Translation</h3>";

string currentDir = System.IO.Directory.GetCurrentDirectory();

if (currentDir == null)
{
    Console.WriteLine("current directory path is null. wtf I cannot continue like this\nPress any key to exit...");
    System.Console.ReadKey();
    Environment.Exit(1);
}

Console.WriteLine($"dir successfully read and it's {currentDir} KEKW");

// read in VNTS file.

String meow = File.ReadAllText(currentDir + "\\vnts.txt");

Console.WriteLine($"vnts file successfully read");

var opt = RegexOptions.Multiline; // needed for ^ anchor to work properly
string oldonly_pt = "^[^>].*?([\n]|$)"; // matches non-updated VNs
string updatedVNs_pt = "^>(.*?)([\n\r]|$)"; // matches updated VNs
string headers_pt = "^("; // matches company/section headers
string remove_pt = "^("; // matches lines we want to remove

// read in headers.txt to find headers to add markup to
try
{
    foreach (String line in File.ReadAllLines(currentDir + "\\headers.txt"))
    {
        headers_pt += line + "|";
    }
    headers_pt = headers_pt.Remove(headers_pt.Length - 1); // remove trailing | character
    headers_pt += ")([\n\r]|$)"; // complete the regex
}
catch (FileNotFoundException)
{
    Console.WriteLine("Can't find file headers.txt\nPress any key to exit...");
    System.Console.ReadKey();
    Environment.Exit(2);
}

// same for remove.txt
try
{
    foreach (String line in File.ReadAllLines(currentDir + "\\remove.txt"))
    {
        remove_pt += line + "|";
    }
    remove_pt = remove_pt.Remove(remove_pt.Length - 1); // remove trailing | character
    remove_pt += ")([\n\r]|$)"; // complete the regex
}
catch (FileNotFoundException)
{
    Console.WriteLine("Can't find file remove.txt\nPress any key to exit...");
    System.Console.ReadKey();
    Environment.Exit(3);
}

Console.WriteLine($"Headers: {headers_pt}");

// do the formatting
String formatted = meow;

formatted = Regex.Replace(formatted, remove_pt, "", opt); // remove unwanted lines
formatted = Regex.Replace(formatted, updatedVNs_pt, "<b>$1</b>", opt); // bold new VNs
formatted = Regex.Replace(formatted, headers_pt, "<h3>$1</h3>\n", opt); // format known headers

formatted = opener + formatted;

Console.WriteLine($"{formatted}");

File.WriteAllText(currentDir + "\\vnts-format.txt", formatted);
Console.WriteLine($"Formatted VNTS written to vnts-format.txt!");
Console.WriteLine($"\nPress any key to quit...");

System.Console.ReadKey();


