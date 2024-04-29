# Capstone: Industrial Design/Manufacturing of a Gaming Device for Helping People with ADHD
This was the capstone project of 6 mechanical engineering students at McGill University. For the purpose of the project, 3 students were responsible for designing the and creating games that could potentially benefit individuals with ADHD and integrating it to a prototype. This repository represents cumulative work of the software team throughout the 2023-2024 year.
This includes 4 games that interact with a Novation Launchpad Mini MK3 and an Arduino Nano using Serial Communication.

The following games that were designed:
- Button Blitz --> Reaction time game
- Drawing Mode --> Creative game
- Music Melody --> Memory game
- Piano Mode   --> Creative game

## Table of Content
1. [Development Enviroment](##1.-development-environment)
2. [Serial Communication](##2.-serial-communication)
3. [Games](##3.-games)

## 1.Development Environement
- Windows OS
- Pre-installation of OpenAL
- Visual Studio 2022 as development tool
- Novation Launchpad Mini MK3

- Arduino Nano (Optionnal)
## 2. Serial Communication
## 3. Games
### Button Blitz
#### Description
Button blitz is a rythm based reaction time game. The target starts from the center of the launchpad and moves towards the outside. The goal is to hit target when it is bright white at the edge of the play area.
There are three song options for the game which increase in speed as the leve is increased.

Song options:
- BGM
- Stayin Alive
- Edance

#### Design
For starters, when the world target is used we are referring to the game elements that are requires the user to hit them by pressing on a button.
These targets are stored in a list called gameTargets. This was done since there was fixed amount of directions that the targets could go in and we wanted to avoid having to manage an ever growing list.
```
  List<Target> gameTargets = new List<Target>();
```
Target is a necessary class since every target needed their own "event" timer for the animations. This class was a first implementation of the idea. An improved iteration of this class was used for the Music Melody game.

All possible targets are stored in 


The songs are handle almost the same way. Since they all have a bpm. 
#### Design
### Drawing Mode
#### Design
### Music Melody
#### Design
### Piano Mode
#### Design




 
