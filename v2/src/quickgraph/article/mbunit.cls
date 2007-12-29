% Class autonomous.cls version 1.0
% Jonathan de Halleux, dehalleux@auto.ucl.ac.be
\NeedsTeXFormat{LaTeX2e}[1995/12/01]
\ProvidesClass{testfu}[2004/08/03 TestFu Project Class]

\LoadClass[10pt]{report}
\RequirePackage{listings}
\RequirePackage{t1enc}
\RequirePackage[usenames]{color}
\RequirePackage{amsmath,amssymb,latexsym}
\RequirePackage{titling}
\RequirePackage{amscd}
\RequirePackage{fancyhdr}
\RequirePackage{graphicx}

% Setting new fonts
%\renewcommand{\sfdefault}{phv}
\renewcommand{\rmdefault}{pnc}
%\renewcommand{\ttdefault}{put}
%\renewcommand{\sfdefault}{ptm}
%\renewcommand{\rmdefault}{phv}
\renewcommand{\ttdefault}{pcr}

% Setting bibliographic style
\bibliographystyle{plain}

% Declaring fonts
\DeclareFixedFont{\TitleFontBN}{T1}{phv}{b}{n}{18pt}
\DeclareFixedFont{\VisuFont}{T1}{phv}{m}{n}{8pt}

% Setting page dimensions
\oddsidemargin = 0cm
\textwidth=16cm
\textheight=22cm
\topmargin=0pt
\marginparwidth=50pt

% Setting titles
\pretitle{\rule[0mm]{\textwidth}{1pt}\begin{flushright}\usefont{T1}{phv}{m}{n}\LARGE TestFu: Art Of Testing \usefont{T1}{phv}{m}{n}\\}
\posttitle{\par\end{flushright}\normalfont}
\preauthor{\begin{flushright}\sffamily\large}
\postauthor{\end{flushright}\vspace{5cm}\normalfont}
\predate{\begin{flushright}}
\postdate{\par\end{flushright}}

% Adding TestFu logo
%\renewcommand{\maketitlehookb}{
%\includegraphics[scale=0.3]{UCL_verti.eps}}

% with this we ensure that the chapter and section headings are in lowercase.
\renewcommand{\chaptermark}[1]{\markboth{\VisuFont #1}{}}
\renewcommand{\sectionmark}[1]{\markright{\VisuFont \thesection\ #1}}
\fancyhf{} % delete current setting for header and footer
\fancyhead[LE,RO]{\bfseries\thepage}
\fancyhead[LO]{\bfseries\rightmark}
\fancyhead[RE]{\bfseries\leftmark}
\renewcommand{\headrulewidth}{0.2pt}
\renewcommand{\footrulewidth}{0pt}
\addtolength{\headheight}{0.5pt} % make space for the rule
\fancypagestyle{plain}{%
\fancyhead{} % get rid of headers on plain pages
\renewcommand{\headrulewidth}{0pt} % and the line
}
\pagestyle{fancy}

\pagenumbering{arabic}

% Defining colors for code coloring
\definecolor{DarkGreen}{rgb}{0,0.4,0}
\definecolor{DarkRed}{rgb}{0.4,0,0}
\definecolor{LightGray}{rgb}{0.7,0.7,0.7}
\definecolor{MyBlue}{rgb}{0,0,1}

% defintion
\newtheorem{remark}{Remark}
\newtheorem{definition}{Definition}

% loading C# language
\lstloadlanguages{[Sharp]C}

% numerotation des lignes
\lstdefinestyle{numbers}{numberstyle=\tiny, stepnumber=1,numbersep=5pt,numbers=left}
\lstdefinestyle{nonumbers}{numbers=none}

% setting cs environment
\lstnewenvironment{cs}[1][]
{\small\lstset{language=[Sharp]C,
        basicstyle=\rmfamily\ttfamily,
        keywordstyle=\tt,
        commentstyle=\color{Gray},
        identifierstyle=,
        stringstyle=\ttfamily\color{Gray},
        showstringspaces=false,
        #1
        }}{\normalsize}

% setting csf environment
\lstnewenvironment{csf}[1][]
{\small\lstset{language=[Sharp]C,
        basicstyle=\rmfamily\ttfamily,
        keywordstyle=\tt,
        commentstyle=\color{Gray},
        identifierstyle=,
        stringstyle=\ttfamily\color{Gray},
        showstringspaces=false,
        frame=shadowbox,
        rulesepcolor=\color{LightGray},
        framexleftmargin=5pt,
        #1
        }}{\normalsize}

\lstnewenvironment{csh}[1][]
{\small\lstset{language=[Sharp]C,
        basicstyle=\small\rmfamily\ttfamily,
        keywordstyle=\tt,
        commentstyle=\color{Gray},
        identifierstyle=,
        stringstyle=\ttfamily\color{Gray},
        showstringspaces=false,
        style=numbers,
        #1
        }}{\normalsize}

\newcommand{\initreport}{
\maketitle
\tableofcontents
\lstset{language=[Sharp]C,
        basicstyle=\usefont{T1}{phv}{m}{n},
        keywordstyle=\tt,
        commentstyle=\color{Gray},
        identifierstyle=,
        stringstyle=\ttfamily\color{Gray},
        showstringspaces=false
}
}

\newcommand{\initappendix}{\appendix}
