# Scatter
Scatter is a static file generator for your blog.

http://silentorbit.com/scatter/

Features include

 * Markdown syntax for pages and blog posts using [MarkdownSharp][1]
 * Minify css and js using the [YUI compressor][2]
 * Generate precompressed versions for use in [nginx gzip-static][3].
 * [Web based editor][4] for local use, using [pagedown for live preview][5]

# Compile

From the command line run `build.sh`.

You can also open the `Scatter.sln` in Visual Studio or MonoDevelop and build it from there.

# Sample blog

A sample blog is available in the `sampleBlog` directory.

Files ending with `.page` become pages.  
Files ending with `.post` become blog post and will be added to the feed according to its date.

Directories with the same base name as page or post files contain additional files such as images or other downloadable content.

# Source code, issues and contributions

https://github.com/hultqvist/scatter


  [1]: https://code.google.com/p/markdownsharp/
  [2]: http://yuicompressor.codeplex.com/
  [3]: http://wiki.nginx.org/HttpGzipStaticModule
  [4]: http://silentorbit.com/notes/Edit/
  [5]: https://code.google.com/p/pagedown/
