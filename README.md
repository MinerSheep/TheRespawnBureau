# TheRespawnBureau
Weeping Angel Momentum based running game, block weeping angel with light


# To make changes in the project
- Create a new branch and name it
- Open the project in File Explorer
- Make changes to any files
- Commit through GitHub Desktops
- Push the commits
- Create a Pull Request

- Pull Request requires someone to review it
- https://www.youtube.com/watch?v=8x6V5IOuXog&pp=ygUkZ2l0aHViIGRlc2t0b3AgcHVsbCByZXF1ZXN0IHR1dG9yaWFs

# This project uses GIT LFS Locking
This can be disabled if you wish, but it is helpful for asset management.
- Project contains FreeLocks.bat (run on Windows) and FreeLocks.sh (run in wsl git bash)
- Assets that have unresolvable merge conflicts (such as .asset) are automatically locked by a script called AutoLock.cs in the Editor folder
- Whoever is in charge of maintaining project files, I would recommend making a reuseable script that runs "git lfs unlock --force --id id" for every locked file