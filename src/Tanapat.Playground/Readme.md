Lesson241027 notes:
- treat Function like first class citizen by using `using static System.Console;`
- I use the `Try<T>` with query expression which `T` MUST be the same type for all the function. so, I create a context class to capture all input/output and pass trough all function in pipeline.
- for the pipeline, we'd better use `File`, `Folder`, `SearchPattern` to wrap `string` because it will prevent mistaken to pass wrong output unintentionally to other functions in pipeline.
- by looking at code, I think I overly use expression !! (but I still wanted to)

Lesson241027's output:

```console
Lesson241027              Content of file := folder *.jpg
Lesson241027              Content of file := folder *.png
Lesson241027              Content of file := folder2 *.jpg
Lesson241027              Content of file := folder2 *.png
Lesson241027Candidate     Content of file := folder *.jpg
Lesson241027Candidate     Content of file := folder *.png
Lesson241027Candidate     Content of file := folder2 *.jpg
Lesson241027Candidate     Content of file := folder2 *.png
```


Lesson241026 notes:
- Avoid return `Seq<string>` because when use `Iter<Seq<string>>`, it will match with `Iter<Seq<A,IEnumerate<A>>` which endup loop through `char` instead of `string`

Lesson241026's output:

```console
 Solution 1
Content := Hello, from D:\folder1\f1.jpg
Content := Hello, from D:\folder1\f2.jpg
Content := Hello, from D:\folder1\f5.jpg
Content := Hello, from D:\folder2\f3.jpg
Content := Hello, from D:\folder2\f4.jpg
Content := Hello, from D:\folder2\f6.jpg
Content := Hello, from D:\folder2\f7.jpg
 Solution 2
Content := Hello, from D:\folder1\f1.jpg
Content := Hello, from D:\folder1\f2.jpg
Content := Hello, from D:\folder1\f5.jpg
Content := Hello, from D:\folder2\f3.jpg
Content := Hello, from D:\folder2\f4.jpg
Content := Hello, from D:\folder2\f6.jpg
Content := Hello, from D:\folder2\f7.jpg
```