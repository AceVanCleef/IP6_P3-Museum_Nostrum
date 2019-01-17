# Lessons learned

## How to write Editor Scripts
- [ExecuteInEditMode will run your script in edit mode](https://blog.theknightsofunity.com/executeineditmode-will-run-script-edit-mode/)

## Tag Comparison: Strings vs. Enum
It is faster to compare enum values than strings. Unity offers a string based tags to identify objects. But enums can be compared with a noticable performance difference. One open question is left to ask: How performant is GetComponent<MyCustomEnumTag> to access a GameObjects custom tag(s)?
- [Squeezing out Performance: Comparing objects](https://forum.unity.com/threads/squeezing-out-performance-comparing-objects.178593/)