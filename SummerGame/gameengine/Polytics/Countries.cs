using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    /// <summary>
    /// An enum for all known countries
    /// </summary>
    /// <remarks>
    /// Some countries will appear twice, mainly because of political changes.
    /// The main principle is like in PanzerGeneralII -- to have a country for each possible flag on a map.
    /// </remarks>
    public enum Countries
    {
        // the BIG guys

        USSR,       // the Soviet Union -- the Great, the Peaceful and the Horrible
        Germany,    // the 3rd Reich, much less peaceful and a little more horrible
        US,         // The United States of North America and some parts of Mexico and Canada 

        // the simply big guys

        UK,         // the UKoGB&NI, also known as the British Empire
        France,     // the one that ended at May 1940
        Japan,      // the one and only Nippon
        Italy,      // Mussolini's Italy has sneaked here while I was not looking
        China,      // not that good at the time. Simply big.

        // some little dolls and puppets

        Spain,      // the 'Franco-Spain', so to speak
        Netherlands, // or is it 'Neverlands'?
        Belgium,    // smashed by germans in two world wars in a row, but rather brave guys
        Finland,    // oh, yes, the "beauty Suomi"
        Poland,     // the one that ended at September 1939
        Romania,    // of some use sometimes, but mostly as worthless as italians
        Hungary,    // sometimes they were even worthy
        Chechoslovakia, // what a long name, what a short history!
        Slovakia,   // did you know they were at war since September 01.1939?
        Manchukuo,  // also known as Manchuria, hell knows why the japs established it
        Mongolia,   // they got lucky to live at 'der Arsch der Welt'

        // British dominions, not even puppets

        Australia,  // the home of bravest cangaroos in the world
        NewZealand, // at that time noone knew where it is, not even australians
        SouthAfrica, // boers and englishmen, united by hatred to the kaffer
        Canada,      // if only they had a chance, they could be good

        // Belonging to the territory of the country

        None     // no man's land
    }
}
