EXTERNAL KnightQuest(questName)
Are you doing alright? You look troubled. #speaker:Eustace #image:PlayerImage
I’m trying to write a letter for my family, but I cannot read nor write. #speaker:Knight #image:KnightImage
I also do not have paper. 
    * I’m sorry to hear that. 
        I’m sorry to hear that#speaker:Eustace #image:PlayerImage
        Can you write? #speaker:Knight #image:KnightImage
        I can. #speaker:Eustace #image:PlayerImage
        Can you help me and buy some paper? #speaker:Knight #image:KnightImage
            **[I can.] *takes out paper and quill* #speaker:Eustace #image:PlayerImage
                ~KnightQuest("KnightsLetter")
                Sure.#speaker:Eustace #image:PlayerImage
            **I can’t. Sorry. #speaker:Eustace #image:PlayerImage
                Oh, okay.  *Player doesn’t get the quest*. #speaker:Knight #image:KnightImage 
    * [I can write your letter]
        I can do it. I can write your letter for you. #speaker:Eustace #image:PlayerImage
        Really? Thank you so much. It means a lot. #speaker:Knight #image:KnightImage
        Do you have any paper?
        No I dont#speaker:Eustace #image:PlayerImage
        Could you help me and get some paper as well?#speaker:Knight #image:KnightImage
            ** No sorry #speaker:Eustace #image:PlayerImage
                Oh okay#speaker:Knight #image:KnightImage
            ** Sure.#speaker:Eustace #image:PlayerImage
                ~KnightQuest("KnightsLetter")
                thank you so much#speaker:Knight #image:KnightImage
	    