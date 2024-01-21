using Day13;

PatternAnalyzer analyzer = new("input.txt");

int notesSummary = analyzer.SummarizePatternNotes();

Console.WriteLine($"Part 1: {notesSummary}");

int notesSummaryWithSmudges = analyzer.SummarizePatternNotesWithSmudges();

Console.WriteLine($"Part 2: {notesSummaryWithSmudges}");