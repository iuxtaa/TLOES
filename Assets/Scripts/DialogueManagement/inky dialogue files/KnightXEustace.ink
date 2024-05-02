EXTERNAL startQuest(questName)
-> main

=== main ===
PC: Are you doing alright? You look troubled.
Knight: I’m trying to write a letter for my family, but I cannot read nor write. I do not have a quill and paper.
    * I’m sorry to hear that. 
        PC: I’m sorry to hear that
        Knight: Can you write? 
        PC: I can. 
        Knight: Can you help me? 
            **[I can.] *takes out paper and quill* 
                Knight: Thank you. I just wanted to tell them that I miss them. I hope they’re doing well. I will be home soon. Someday. 
                PC: *writes and finishes* Here you go. 
                Knight: Thank you. I appreciate it. Can I take you to the chapel? I’d like you to meet the priest. 
                PC: Sure.
                ->DONE
            **I can’t. Sorry. 
                Oh, okay.  *Player doesn’t get the quest*.  
                ->DONE
    * I can do it. I can write your letter for you. 
        PC: I can do it. I can write your letter for you. 
        Knight: Really? Thank you so much. It means a lot. 
        PC: What do you want to say to them? *takes out paper and quill* I can use a bit of the paper I have. 
        Knight: This means everything to me. I just wanted to tell them that I miss them. I hope they’re doing well. I will be home soon. Someday. 
        PC: *writes and finishes* Here you go.
        Knight: Thank you. I appreciate it. Can I take you to the chapel? I’d like you to meet the priest. 
        PC: Sure.
        ~startQuest("sellingQuest")
        ->DONE
        
    