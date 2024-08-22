# CMS HackerHero

This project is a minimal Content Management System (CMS) tailored for ZoCom Company, designed to streamline content creation, management, and publishing with a focus on simplicity and efficiency.

<h1> Git workflow </h1>

### Considerations 

For a debate regarding "merge" vs "rebase" see this link: https://www.atlassian.com/git/articles/git-team-workflows-merge-or-rebase

When starting a new application:

- create development branch from main

When implementing a new feature:

- pull from development Remote to development Local, to have the latest updates
- create feature branch from development
- do some commits in the feature branch
- test your code

When your feature is complete and you want to merge to development, make sure there are no discrepancies between your branch and development before merging back into development:

- pull from development Remote to development Local, to have the latest updates
- merge from development to respective feature branch to have the latest updates from development
- resolve conflicts if necessary 
- test your code
- make a pull request from feature to development
- test your code
