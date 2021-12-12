# Jackdaw
## ASP.NET 6.0 MVC Development

<hr />

[![GitHub license](https://img.shields.io/github/license/cdcavell/Jackdaw)](https://github.com/cdcavell/Jackdaw/blob/main/LICENSE)
![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/cdcavell/Jackdaw)
![GitHub top language](https://img.shields.io/github/languages/top/cdcavell/Jackdaw)
![GitHub language count](https://img.shields.io/github/languages/count/cdcavell/Jackdaw)
[![CodeQL Analysis](https://github.com/cdcavell/Jackdaw/workflows/CodeQL%20Analysis/badge.svg)](https://github.com/cdcavell/Jackdaw/actions?query=workflow%3A%22CodeQL+Analysis%22)

<hr />

Project incorporates generation of markdown files in Source\Xml folder, during project builds, from comment syntax of source code, through console application XmlToMarkdown. Documentation changes are maintained in a [wiki submodule](https://git-scm.com/docs/git-submodule) that is also updated during project build.

Target Framework is [ASP.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0). Developed and built in a Windows environment utilizing [Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/) source-code editor. Repository is [Git](https://git-scm.com/) utilizing [git-flow](https://github.com/nvie/gitflow) extension to provide high-level repository operations for [Vincent Driessen's branching model](https://nvie.com/posts/a-successful-git-branching-model/).

This work is [licensed](https://github.com/cdcavell/Jackdaw/blob/main/LICENSE) under the [MIT License](https://opensource.org/licenses/MIT).

Source Code documentation is found in repository [Wiki](https://github.com/cdcavell/Jackdaw/wiki) section. 

<hr />

_If you are cloning this repository, please enter commands as follows:_

```
$ git clone --recurse-submodules https://github.com/cdcavell/Jackdaw.git

$ cd Jackdaw

$ git flow init -d
```

<hr />

| Build | Date | Description |
|-------|------|-------------|
| 0.0.0.1 | 12/12/2021 | __Initial Development__ |
