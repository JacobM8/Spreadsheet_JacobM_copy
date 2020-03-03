Authors:    Jacob Morrison and James Gibb
Date:       February 29, 2020
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  jamesrgibb
Repo:       https://github.com/uofu-cs3500-spring20/assignment-six-completed-spreadsheet-j-squared
Commit #:   The commit identifier that goes along with the final submitted code.
Assignment: Assignment #6 - Spreadsheet Front-End Graphical User Interface
Copyright:  CS 3500 and Jacob Morrison and James Gibb - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:

    I, James Gibb have disability accommodations that allow for flexible deadlines with assignments without penalty. 
    It was an interesting assignment to interact with the GUI and the grid API. We had issues implementing arrow keys 
    and the background worker. We were able to make the background worker functional and found some interesting methods in 
    the DrawingPanel class.

2. Assignment Specific Topics:

- Learning how to follow (somewhat) complex Requirements and Specifications. 
- Creating GUIs representing the View/Controller part of the MVC architecture.
- Integrating the Model in the MVC architecture.
- Using a Task Management System (GitHub Projects) to track assigned work and completion.
*(Optional, but encouraged) Using pair programming practices, branching, and merging.

3. Consulted Peers:

Varun
Logan
Michael
Jolie
David Randall
Nate Koeliker
Corbin Gurnee
Misty 
Jay Morrison

4. References:

    1. MS Docs Creating Read Only Text-Box: https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-create-a-read-only-text-box-windows-forms
    2. MS Docs Forms and Frameworks: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.formclosing?view=netframework-4.8
    3. Close events using X: https://stackoverflow.com/questions/1669318/override-standard-close-x-button-in-a-windows-form
    4. Rounding to two decimals with currency: https://stackoverflow.com/questions/2357855/round-double-in-two-decimal-places-in-c
    5. MS Docs  Drawing Color: https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color?view=netframework-4.8
    6. MS Doc Windows.color: https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=netframework-4.8
    7. Use of https://app.pluralsight.com/course-player?clipId=c6d597ba-a69d-4536-a3a8-9520b998b5ca

5. Examples of Good Software Practice:

    We created several helper methods to avoid re-using code and for easy reference. We commented our code clearly, to help
    others navigate and understand the code better. We implemented Liskovs method through the different spreadsheet and grid_widget 
    objects to accomplish the goals of the assingment.

6. GitHub Repo and Commit Number

    - *** TODO *** update Please include a short section near the top of the README that indicates the github repository URL as well as the commit number associated with 
    the work you handin to GradeScope.

7. Partnership Information

    James Gibb completed the save, open, and dark mode functions with little help from Jacob.
    Jacob Morrison completed the helper methods getCellLocation, getCellName, and the font color. 
    The rest of the methods were completed via paired programming.

8. Branching
    
    James made the following branches helpMenu, SaveFile, OpenFunction, AdditionalFeature
    Jacob made the following branches formulaFormatErrorInTextBox, BGWorker, AddFeatureFillAndColor
    
    -There were couple of instances were we had trouble merging into the master branch. We discovered our problem was uncommited changes by one of us. There 
    was an instance were had reverted and solving that took an hour or so. Overall this assignment helped us get better at using git.

    Dark Mode: dc0e8e2b76ab1dfe95f271791b813ecc94e5415c
    Changed Font Color: 853a6cfb138fc424e34eda0d47c9f5c0c9677676
    Open and Divide by Zero: 6c6ca8ea81bfefcb6e2022a52ee71d086648d096
    Save and Exit: 4740e0b1f179d7be002ad5df88215e00db89787e
    Close Menu: e33ffcae3ffa4848641cf43be69486920a0a6507
    Help Menu: 9858b21960f4dad4f8d3e2ed0872b95555a6f3c7
    Circular Exceptions: cdad45285da742917ebc291827a87863d59d527a
    Click and Key Down: d99709efba427f2df948b656c115a9a811738686
    Get Cell Name: 2ca1be26c989ccc1ba8926fee60c77de36cea869
    Table of Contents: 72cf531630e9266bd9a6b3a31eee0d6a346ce1b8
 
9. Additional Features and Design Decisions

    The spreadsheet was designed similar to the Excel Application, with a green background. We chose to design the layout for the
    Cell Label, Cell Value, Cell Contents ordered vertically. The background worker is displayed as the blue circle until the program
    finishes executing (we used the .sleep funtion to better show the implementation of our backgroundWorker). We used several helper 
    methods with the background worker to ensure it worked efficiently and our code was well organized. We also used several helper 
    methods throughout the GUI application to get and set the appropriate values with the cell, as well as notify the user of any possible 
    input errors. In addition to the functionality of the Spreadsheet GUI, we implemented two extra features:
    
    1. Changes the color of the font according to the users preference, this is done by clicking the drop down menu titled Font Color.
       The chosen color is then populated as the new font color.
    2. Also we impelented a dark mode for the entire GUI. By clicking the button at the top of the GUI labeled "Dark Mode" the user can
       toggle between a lighter and darker theme according to their preference.
    

10. Best (Team) Practices

    Our partnership worked effectively from the beginning by doing thorough code reviews to make a few changes to start off with a solid foundation. We were also
    efficent in helping each other identify when we can implement helper methods, use better naming conventions, and include interline comments when a function
    is completed. 
    One area of improvement that would have helped us save time and be more efficient was moving on from a problem when we couldn't find a solution. One example is
    we tried to implement the ability to move around the grid with arrow keys but could not figure out a way even after talking to multiple TAs. We estimated that we
    spent 7 hours on trying to figure out the arrow keys. We could have stopped earlier to move on to other portions of our assignment. 

11. How We Ensured Correctness of Our Code
    
    We opted to test our code manually as we completed each method. After a method was completed we tested each method for correctness and completeness. 
    We did this by testing for edge cases and ensuring that errors were thrown in the appropriate areas. The GUI was tested manually after each 
    implementation of a new feature. We also tested for responsiveness of the background worker, we tested different sizes of files to see how the program would 
    handle large files. We also tested the background worker's ability to work independently of the spreadsheet by testing if the GUI was able to move 
    and respond while calculating large values.

6. Estimated Time: PS5 -  20 hours   

7. Actual Time:    PS5 -  30                   
    Learning:             5 hours               
    Programming:          10 hours
    Debugging:            15 hours

------------------------------------------------------------------------------------------------------


