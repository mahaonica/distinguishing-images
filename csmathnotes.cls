\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{csmathnotes}[2021/05/18]
\LoadClass[a5paper]{article}

\RequirePackage[T2A]{fontenc}
\RequirePackage[utf8]{inputenc}
\RequirePackage[english,russian]{babel}

\RequirePackage{indentfirst}
\RequirePackage[colorlinks=true,urlcolor=blue,linkcolor=blue,citecolor=blue]{hyperref}
\RequirePackage[margin=2cm,bottom=2.5cm,headsep=3mm]{geometry}
\RequirePackage{fancyhdr}
\RequirePackage{graphicx}
\RequirePackage{enumitem}
\RequirePackage{url}
\RequirePackage{amsmath}
\RequirePackage{amssymb}
\RequirePackage{amsthm}
\RequirePackage{xstring}
\RequirePackage{cmap}
\RequirePackage[backend=biber,style=gost-numeric,language=auto,autolang=other,sorting=none,movenames=false]{biblatex}

\pagestyle{fancy}
\lhead{\it \foreignlanguage{russian}{Заметки по информатике и математике}}
\chead{}
\rhead{\it \foreignlanguage{russian}{Выпуск 13}}
\lfoot{}
\cfoot{\thepage}
\rfoot{}
\renewcommand{\headrulewidth}{0.4pt}

\newtheorem{theorem}{Теорема}
\newtheorem{lemma}{Лемма}
\newtheorem{corollary}{Следствие}

\setlist{nosep}

\clubpenalty=10000 
\widowpenalty=10000

\let\oldbibliography\thebibliography
\renewcommand{\thebibliography}[1]{%
  \oldbibliography{#1}%
  \setlength{\itemsep}{-3pt}%
}
\renewcommand*{\newblockpunct}{\addperiod\space\bibsentence}

\let\footnoterule\relax

\newcommand{\keywords}[1]{\emph{Ключевые слова:} #1}

\gdef\@author{}
\gdef\@authorrev{}
\gdef\@email{}
\newcommand\udc[1]{\gdef\@udc{#1}}
\renewcommand{\author}[1]{
  \StrBefore{#1}{ }[\lastname]
  \StrBehind{#1}{ }[\nametail]
  \StrBefore{\nametail}{ }[\firstname]
  \StrBehind{\nametail}{ }[\middlename]
  \StrLeft{\firstname}{1}[\firstinitial]
  \StrLeft{\middlename}{1}[\middleinitial]
  \def\initials{\firstinitial.\,\middleinitial.}
  \ifx\@author\empty
    \xdef\@author{\initials~\lastname}
    \xdef\@authorrev{\lastname~\initials}
  \else
    \xdef\@author{\@author, \initials~\lastname}
    \xdef\@authorrev{\@authorrev, \lastname~\initials}
  \fi
}
\newcommand{\advisor}[2]{}
\newcommand{\advisoremail}[1]{}
\newcommand{\position}[1]{} % used in author's contract only
\newcommand{\affiliation}[1]{\gdef\@affiliation{#1}} % FIXME
\newcommand{\email}[1]{
  \ifx\@email\empty
    \xdef\@email{#1}
  \else
    \xdef\@email{\@email, #1}
  \fi  
}

\renewcommand\section{\@startsection {section}{1}{\z@}%
  {-2ex \@plus -1ex \@minus -.2ex}%
  {0.5ex \@plus.2ex}%
  {\normalfont\bfseries}}
\ifdefined\firstpagenum
\setcounter{page}{\firstpagenum}
\fi

\def\@maketitle{%
  \newpage
  \null
  \vspace*{-5mm}
  {\small\noindent УДК \@udc}
  \begin{center}%
    \let\thefootnote\relax%
    \footnote{\textcopyright\ \@authorrev, 2021}%
    {\bf \@title \par}%
    \vskip 0.5em%
    {
      \lineskip .5em%
      \begin{tabular}[t]{c}%
        \@author\\
        \@affiliation\\
        E-mail: \@email%
      \end{tabular}\par}%
  \end{center}%
  \par
}
\def\@thanks{%
  \thispagestyle{fancy}
}
