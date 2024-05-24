EXTERNAL beginQuest(questName)
EXTERNAL completeQuest(questName)
Are you doing alright? You look troubled. #speaker:Eustace #image:PlayerImage
I’m trying to write a letter for my family, but I cannot read nor write. #speaker:Knight #image:KnightImage
I do not have a quill and paper. 
    * I’m sorry to hear that. 
        I’m sorry to hear that#speaker:Eustace #image:PlayerImage
        Can you write? #speaker:Knight #image:KnightImage
        I can. #speaker:Eustace #image:PlayerImage
        Can you help me? #speaker:Knight #image:KnightImage
            **[I can.] *takes out paper and quill* #speaker:Eustace #image:PlayerImage
                ~beginQuest("KnightsLetter")
                Thank you. I just wanted to tell them that I miss them. I hope they’re doing well. I will be home soon. Someday. #speaker:Knight #image:KnightImage
                WRITES A LETTER***  Here you go. #speaker:Eustace #image:PlayerImage
                Thank you. I appreciate it. Can I take you to the chapel? I’d like you to meet the priest.#speaker:Knight #image:KnightImage 
                Sure.#speaker:Eustace #image:PlayerImage
		        ~completeQuest("KnightsLetter")
            **I can’t. Sorry. #speaker:Eustace #image:PlayerImage
                Oh, okay.  *Player doesn’t get the quest*. #speaker:Knight #image:KnightImage 
    * I can write your letter for you. 
        I can do it. I can write your letter for you. #speaker:Eustace #image:PlayerImage
	    ~beginQuest("KnightsLetter")
        Really? Thank you so much. It means a lot. #speaker:Knight #image:KnightImage
        What do you want to say to them? *takes out paper and quill* I can use a bit of the paper I have. #speaker:Eustace #image:PlayerImage
        This means everything to me. I just wanted to tell them that I miss them. I hope they’re doing well. I will be home soon. Someday. #speaker:Knight #image:KnightImage
        WRITES A LETTER*** Here you go.#speaker:Eustace #image:PlayerImage
        Thank you. I appreciate it. Can I take you to the chapel? I’d like you to meet the priest. #speaker:Knight #image:KnightImage
        Sure.#speaker:Eustace #image:PlayerImage
	    ~completeQuest("KnightsLetter")