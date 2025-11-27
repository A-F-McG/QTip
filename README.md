# QTip README

## Instructions for running the application:

Clone the repo, then:

- To run: `docker compose up` inside the top-level directory (QTip) where the compose file is

- View the application on: `localhost:3000`

- View the database on: `localhost:8081`

## Architectural decisions and assumptions

### Backend:

- C#/ASP.NET minimal API. The task has a small scope and I always prefer simplicity rather than overengineering

- Set the regex in the constructors of the various pii detector classes so that it's not regenerated every submission as I think that can get a little expensive

- Tokenised text is built using a guid so that it's unique for every email

- Moved all the added services into a separate file to try and keep the main Program file a little more readable

- Tried to keep a minimal amount of files, as simplicity and readability for this small scope was my goal rather than a proper business-style architecture. E.g. the DatabaseOperations file is probably not what I'd have in company code as it includes logic as well as the database operations.

### Database:

- Using SQLite. Again, went for the easiest option to set up and simplest option for others to access.

- In the classifications table, I'm storing the original value as an encrypted blob because otherwise it wouldn't be very secure if someone got access to the database and could read everyone's personal information (even if that person was a company employee with accidental access to the db rather than someone hacking in)

- Unique emails per submission are stored in the database. This is done 1. in DetectDistinct, and so I only create tokens for unique emails, and 2. the database has a constraint saying TokenizedPii should be unique, which means even if multiple submissions with the same email are made at the same time, no duplicates will be entered into the db. Similarly, EncryptedPii in unique in the database.

- I considered a foreign key linking the submission text to the various Pii entries to enable simpler queries to reconstruct the message and to make it easy to delete the pii classifications if a user wanted to remove their submissions from our db, however since each email should be unique in the db with a unique token, this setup didn't end up making sense so I abandoned it, plus reconstructing the message isn't explicitly needed here

  #### Bug:

  This unique email situation works per submission, but I know that right now if you send the same email through in a second submission, it's going to build the token and encrypted text for it, try to add it to the classifications table (which won't happen because of the unique constraint), but then it will still input the tokenised text into the tokenised submissions table with the generated token (instead of using the existing one in the classifications table).

  The problem is I need to first check whether the email/pii has already been added to the classifications table, but I can't search via the tokenised text because I don't know it, and I can't search using the ecrypted bytes, because 1. I think that's probably a little odd and 2. I couldn't anyway as I don't know the random initialisation vector without finding the entry in the database.

  I think the solution is to use a hash of some sort that you also save in the database and can recreate and search by. I'm not super familiar with that but would like to look into it more and learn how it works; cryptography was actually my favourite module at university so I'm sure I understood all this once upon a time!

### Frontend:

- Using React and simple SASS. React is good for the bit of interactivity on the page (e.g. showing the annotated text with tooltips on the fly). I'm very familiar with NextJS, and noticed Qala also uses it, but that seemed overkill for this project since I don't need any routing, it's not deployed so server side rendering isn't needed and I'm not using the server actions/backend capabilities of NextJS. I've tried to keep a minimal amount of files, while still having things be readable without too much in one file.

- Using unnamed function rather than arrow functions. This is just personal preference because I like making use of the the hoisting so that functions can be written at the bottom of the file - I find it more readable. But I think consistency is more important than my slight personal preference, so within a company codebase I'd agree on/follow existing conventions.

- I've chosen to display the text underneath the form with pii underlined and with tooltips. This is for accessibility reasons as you should be able to trigger the tooltips using your keyboard without your mouse (so that people with various disabilities, e.g. blindness and using a screen reader or someone with tremors who struggles with dexterity) can also access the information.

### Testing:

- I've used Jest and xUnit to unit test the regex patterns because I love a good test and I'm familiar with those libraries so it was quick to implement. I didn't write as many tests as usual (I often start with them/write the code and test in small increments together), and I'd have liked to add end to end testing too, maybe using Playwright.

### Things I wouldn't do in production:

- Hardcoded the connectionstring since it's just running locally, obviously wouldn't do that in production as it's not secure! I'd put it in the environment variables as we'd probably have different databases for different environemnts

- Harcoded the encryption key (used to encrypt the pii). Obviously also a terrible idea! I'd keep this somewhere like Azure Key Vault to keep it safe

## Trade-offs or shortcuts taken

- I'm sure there are a bunch of edge cases for email formatting. I've used a simple-ish regex which catches the basic email format, but I haven't deep dived into which symbols are and aren't allowed in emails so I may well be catching things that aren't legitimate emails. I've also assumed that the email starts and ends with a word boundary, as things were starting to get complicated when I tried to account for people not leaving spaces around the email, which I'd have had to have more of a think about to find a solution. I've also used an oversimplified phone regex

- I'd add better error handing with more time, both on the frontend and backend

- Styling is quite basic

- I generally push smalller git commits (& PRs) to make it easier to understand what's changed, but I was having fun and got too into the project :)

## (Optional) Implementation notes for the optional extension

- Implemented the backend (you can submit and view 10-digit phone numbers added to the classifications table):
  I created an IPiiDetector interface with a DetectDistinct function and the type (of pii detector, e.g. email). Because I've added IPiiDetector to the app services, and I'm injecting them using dependency injection in Program, it means for any new pii I want to detect, all I'd need to do is create a new class that implements the interface and register it in the services which makes it clean and easy to keep track of what pii we have in the project.

- Frontend plans:
  if I spent more time on it, I'd have created an interface similar to IPiiDetector including 1. the Pii regex (except here I want to detect all emails, not distinct ones) and 2. the pii type. Then either in App, or a separate file if the project grew bigger, I'd define a list of my piiDetectors, loop through them to find the pii text and type, passing that into wrapTextWithTooltips which I'd tweak to take in an array. I'd do it this way so that again, there was one point of reference for which pii is being searched for (in the defined list), and one folder where I'd create the different pii functions + tests so it's easy to find everything.
