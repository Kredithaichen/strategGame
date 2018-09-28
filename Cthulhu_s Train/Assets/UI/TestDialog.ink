
=== character1 ===

= entry

* { not character1.first_encounter} -> character1.first_encounter
+ { character1.first_encounter } -> character1.continue

= first_encounter
Hello, stranger! My name is "First Character".
* 	[Hello back!]
	Nice to see you around! 
	-> character1.regular_encounter
*	[Bye]
	See you around! # end
	-> DONE

= continue
Welcome back!
-> character1.regular_encounter

= regular_encounter
How is it going?
+	[Good]
	Nice # end
	-> DONE
+	[None of your business]
	Chill, dude # end
	-> DONE
+	[Bad]
	Not nice # end
	-> DONE

=== character2 ===

= entry

* { not character2.first_encounter} -> character2.first_encounter
+ { character2.first_encounter } -> character2.continue

= first_encounter
Hello, stranger! My name is "Second Character".
* 	[Hello back!]
	Nice to see you around! 
	-> character2.regular_encounter
*	[Bye]
	See you around! # end 
	-> DONE

= continue
Welcome back!
-> character2.regular_encounter

= regular_encounter
What is on your mind?
+	[Who are you?]
	I am Second Character. 
	-> character2.regular_encounter
+	[What is your purpose here?]
	I demonstrate dialog options. 
	-> character2.regular_encounter
+	[Bye]
	See you around! # end 
	-> DONE