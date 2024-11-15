INCLUDE Globals.ink
EXTERNAL giveItem(itemName)

Hey, you. You're finally awake!
-> main

=== main ===
What brings you here?
+ [Where am I?]
    Welcome to Bytla!
    You'll probably end up with severe brain damage here
    This is a dungeon with numerous underground levels.
    Each levels are surrounded by enemies and a Boss.
    You'll have to fight them to proceed downwards.
    ->ask
+ [Who are you?]
    youknowhowloongivebeenwaitingforthis?
    hooimboutamakeanameformyselfyeah.
    mynamesjeff
    ->ask
+ [Help me]
    i'll sho u da wei
    {bookName == "": -> giveBook("Book of Byte Quest") | -> gotBook}
    

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
    ogge
    -> END

=== gotBook ===
I already gib u da book
No mor
-> END