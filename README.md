# Unitversal

Unitversal is an "intelligent" app that performs unit conversions based on a
text input by using information stored in a SQLite database. There are currently
over 1000 units in the database with more to be added in the future.

# Usage

There are four query types Unitversal recognizes: exploring all units in the
database, information about a specific unit, converting a unit to all units of
the same type and converting between two units.

To explore all units in the database, type "explore" into the search box. This
can also be done by clicking the "Explore All Units" button in the settings.

<p align="center">
   <img src="https://github.com/Member-Of-The-Reals/Unitversal/blob/main/Resources/Screenshots/Explore%20Query.png" alt="Explore Query Screenshot"/>
</p>

To get information about a specific unit, type in the search box the unit name
and double click the unit once it appears in the list below.

>Example query: meter

<p align="center">
   <img src="https://github.com/Member-Of-The-Reals/Unitversal/blob/main/Resources/Screenshots/Information%20Query.png" alt="Information Query Screenshot"/>
</p>

<p align="center">
   <img src="https://github.com/Member-Of-The-Reals/Unitversal/blob/main/Resources/Screenshots/Unit%20Information.png" alt="Unit Information Screenshot"/>
</p>

To convert one unit to all corresponding units, type in the search box a
magnitude followed by the unit name.

>Example query: 1 meter

<p align="center">
   <img src="https://github.com/Member-Of-The-Reals/Unitversal/blob/main/Resources/Screenshots/Convert%20All%20Query.png" alt="Convert All Query Screenshot"/>
</p>

To convert between two units, type in the search box a magnitude followed by the
names of the two units where the names are separated by the word "to".

>Example query: 1 meter to foot

<p align="center">
   <img src="https://github.com/Member-Of-The-Reals/Unitversal/blob/main/Resources/Screenshots/Convert%20To%20Query.png" alt="Convert To Query Screenshot"/>
</p>

Symbols, abbreviations, and minor misspellings will still be recognized by the
app.

Additionally, Unitversal can perform currency conversions as well. The currency
exchange rates are from the US Treasury Reporting Rates of Exchange API, and can
be updated manually from the settings or during startup. Note that the exchange
rates are updated every fiscal quarter with minor updates for deviations greater
than 10% and as such there is no need to update frequently.

# License

Unitversal and its source code is licensed and distributed under GNU General
Public License v3.0. See LICENSE.txt for license text.

Contact: memberofthereals@gmail.com

© 2022 Member of the Reals
