# define-define

Source code for the `define-define` protocol, our submission to the Fall 2019 [Aesthetics of Prosthetics](https://www.prostheticsexhibition.com/) exhibition.

## Repository structure

**/api**: rhino compute backend for geometric logic and version control

**/app**: vue frontend for drawing display and user-override interface

**/fct**: firebase cloud functions that handles relationship between app, api, and database

## How to run

- Install [Docker](https://www.docker.com/)
- Switch to [Windows containers](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers)
- Run `docker-compose up`

## Abstract

We, the designers, have devised a protocol for the creation of some drawing.

Where Sol LeWitt was so obsessed with abdicating control over outcome, we believe that the ever-present threat of machine intelligence asks us to maintain a greater degree of responsibility. Participation, as virtuous as it may seem, is a red herring. Machine programming languages offer us the opportunity to explicitly define our intent. Alexander and Price were correct to identify this as a chance to expose sites of user input; they were incorrect to step away.

In this game, users are provided with our instructions as an interface. They may override our definitions through drawing. At the same time, we are displaying live output from our automated system that uses these definitions. But both are just visuals; the project exists in the protocol that directs the users and machines in concert. We have translated our intent into this protocol and hosted it on the internet.

We defer specificity in language, not action. We define define.

We aim to find a means to marry indeterminate outcome with a high degree of designer responsibility and control. This collective language prosthetic is one attempt. We believe, as designers, that our future relationship with machine intelligence will be defined by this paradox.
