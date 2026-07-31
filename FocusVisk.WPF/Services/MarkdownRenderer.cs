using Markdig;

namespace FocusVisk.Services;

public static class MarkdownRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public static string ToHtml(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown)) return "<p class=\"markdown-empty\">nada por aqui ainda belê</p>";
        return Markdown.ToHtml(markdown, Pipeline);
    }
}