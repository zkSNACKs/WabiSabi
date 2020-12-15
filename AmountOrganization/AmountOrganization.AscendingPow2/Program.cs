﻿using System;
using System.Linq;
using AmountOrganization;
using AmountOrganization.DescPow2;

var inputCount = 50;
var userCount = 40;

var preRandomAmounts = Sample.Amounts.RandomElements(inputCount);
var preGroups = preRandomAmounts.RandomGroups(userCount);

var preMixer = new DescPow2Mixer();
var preMix = (preMixer as IMixer).CompleteMix(preGroups);

var remixCount = (inputCount / 10) * 3;
var randomAmounts = Sample.Amounts.RandomElements(inputCount - remixCount).Concat(preMix.SelectMany(x => x).RandomElements(remixCount));
var groups = randomAmounts.RandomGroups(userCount).ToArray();
var mixer = new DescPow2Mixer();
var mix = (mixer as IMixer).CompleteMix(groups).Select(x => x.ToArray()).ToArray();

var outputCount = mix.Sum(x => x.Count());
var inputAmount = groups.SelectMany(x => x).Sum();
var outputAmount = mix.SelectMany(x => x).Sum();
var fee = inputAmount - outputAmount;
var size = inputCount * mixer.InputSize + outputCount * mixer.OutputSize;
var feeRate = (fee / size).ToSats();

Console.WriteLine();
Console.WriteLine($"Number of users:\t{userCount}");
Console.WriteLine($"Number of inputs:\t{inputCount}");
Console.WriteLine($"Number of outputs:\t{outputCount}");
Console.WriteLine($"Total in:\t\t{inputAmount} BTC");
Console.WriteLine($"Fee paid:\t\t{fee} BTC");
Console.WriteLine($"Size:\t\t\t{size} vbyte");
Console.WriteLine($"Fee rate:\t\t{feeRate} sats/vbyte");

Console.WriteLine();

foreach (var (value, count, unique) in groups
    .GetIndistinguishable()
    .OrderBy(x => x.value))
{
    if (count == 1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    var displayResult = count.ToString();
    if (count != unique)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        displayResult = $"{unique}/{count} unique/total";
    }
    Console.WriteLine($"There are {displayResult} occurrences of\t{value} BTC input.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

Console.WriteLine();

foreach (var (value, count, unique) in mix
    .GetIndistinguishable()
    .OrderBy(x => x.value))
{
    if (count == 1)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    var displayResult = count.ToString();
    if (count != unique)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        displayResult = $"{unique}/{count} unique/total";
    }
    Console.WriteLine($"There are {displayResult} occurrences of\t{value} BTC output.");
    Console.ForegroundColor = ConsoleColor.Gray;
}

Console.ReadLine();
