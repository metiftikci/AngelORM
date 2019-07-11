# 1.1.3
* Fixed enum equivalent on where expression

# 1.1.2

* Allow IEnumerable<int>.Contains() method in where expression to generate query like '[Column] IN (1,2,3,4)'

# 1.1.1

* Added `Nulable<T>` and `Enum` type column tests to support that features
* `OnQueryExecuting` method added to `SelectOperation<T>`
