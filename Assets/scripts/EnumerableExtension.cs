﻿using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtension {
    public static T PickRandom<T>(this IEnumerable<T> source) {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, double percentage) {
        var count = source.Count();
        // Clamping percentage to be a double in range 0-1
        percentage = (percentage < 0) ? 0 : Math.Min(percentage, 1); 
        return source.Shuffle().Take((int)Math.Floor(count * percentage));
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {
        return source.OrderBy(x => Guid.NewGuid());
    }
}