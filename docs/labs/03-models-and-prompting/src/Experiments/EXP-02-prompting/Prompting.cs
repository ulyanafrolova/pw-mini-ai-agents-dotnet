// EXP-02: wpływ zero-shot, one-shot i few-shot na jakość ekstrakcji
public static class Prompting
{
    private const string DefaultModel = "gemma3:4b";

    public static async Task RunAsync(OllamaClient client, string? model)
    {
        model ??= DefaultModel;

        Console.WriteLine($"\n=== Model: {model} ===");

        var examples = TestData.PromptExamples;

        var variants = new (string Name, Func<string, string> Prompt)[]
        {
            ("zero-shot", note => Extraction.Prompt(note)),
            ("one-shot", note => Extraction.Prompt(note, examples.Take(1))),
            ("few-shot", note => Extraction.Prompt(note, examples))
        };

        foreach (var (name, buildPrompt) in variants)
        {
            Console.WriteLine($"\nWariant: {name}");

            var metrics = await Extraction.EvaluateAsync(
                client,
                model,
                TestData.Examples,
                buildPrompt);

            ResultsCsv.Append("EXP-02", model, name, 0, metrics);
        }
    }
}
