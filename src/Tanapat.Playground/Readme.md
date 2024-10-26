
Lesson241026 notes
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