INCLUDE Globals.ink
EXTERNAL giveItem(itemName)

Hey, you. You're finally awake!
-> main

=== main ===
What brings you here?
+ [Where am I?]
    Welcome to Bytes' Dungeon!
    You've probably ended up here because the LORD TASM is sad.
    Maybe because he's hard to deal with...
    Anyway
    This is a dungeon with numerous blocks of areas.
    Each areas are surrounded by enemies and a boss.
    You'll have to fight them if you want to get acknowledged and obtain a badge.
    ->ask
+ [Who are you?]
    youknowhowloongivebeenwaitingforthis?
    hooimboutamakeanameformyselfyeah.
    mynamesjeff
    ->ask
+ [Help]
    Take this with you...
    {bookName == "": -> giveBook("Book of The First Byte") | -> gotBook}
    

=== giveBook(book) ===
~ bookName = book
Obtained {book}!
~ giveItem(book)
->ask

=== ask ===
Do you need anything else?
+ [Yes]
    ->main
+ [No]
    -> END

=== gotBook ===
I already gib u da book
No mor
-> END