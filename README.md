# National Tuberculosis Surveillance system
See the [ntbs-service](./ntbs-service) folder for the application code of the NTBS web server.
 
## Sub-modules
This repository contains sub-modules. When cloning please use this command: 
`git clone --recurse-submodules git@github.com:publichealthengland/ntbs_Beta.git`.

If you have already cloned, then please use this command to initialise and update your sub-modules: 
`git submodule update --init --recursive`.

### Using https for git
The above instructions assume you are authenticating git using an SSH key. If you'd rather use https, then:

1. Run `git clone https://github.com/publichealthengland/ntbs_Beta.git` 
1. Replace `git@github.com:Softwire/frontend-dotnetcore.git` with `https://github.com/Softwire/frontend-dotnetcore.git` 
inside of the `.gitmodules` file
1. Run `git submodule update --init --recursive`