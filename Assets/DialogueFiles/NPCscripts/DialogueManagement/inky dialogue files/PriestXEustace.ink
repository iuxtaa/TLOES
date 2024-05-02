EXTERNAL startQuest(questName)
Priest: Oh, hello there, farmer’s son. 
    *[hello] PC: Hello
Priest: I heard you helped our knight. Thank you.
PC: It was nothing.
Priest: To him, it was everything. 
PC: I see.
Priest: You know, the holy well of our Church has dried up because of the drought. We usually used it for our holy water to grant blessings to things. 
Do you think you can help us, farmer’s son?
    **[Ofcourse] PC: Ofcourse
    Priest: Thank you. If you go far east from here, you will find a ruined temple.
    There is a well, and I need you to fetch the water from that well and bring it back.
    PC: Okay 
    ~startQuest("PriestsHolyWater")
    ** [I'm not sure, sorry.] PC: I'm not sure, sorry.
    Priest: We need your help, please. 
    PC: Fine. I’ll do it. 
    Priest: Know that your efforts are appreciated, farmer’s son. 
    ~startQuest("PriestsHolyWater")