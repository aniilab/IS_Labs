from typing import Callable

class Character:
    def __init__(self, name):
        self.name = name

    def __str__(self):
        return f"{self.name}"

    def __repr__(self):
        return f"Character with name {self.name}"

# Define characters
harry = Character(name="Harry")
hermione = Character(name="Hermione")
ron = Character(name="Ron")
draco = Character(name="Draco")
voldemort = Character(name="Voldemort")

characters = [harry, hermione, ron, draco, voldemort]

# Hogwarts predicates
has_wizard_parents = "has_wizard_parents"
is_brave = "is_brave"
is_clever = "is_clever"
does_fight_evil = "does_fight_evil"

# Rules
def is_good_wizard(character: Character):
    has_wizard_parents_fact = fact_dictionary[has_wizard_parents].get(character)
    is_brave_fact = fact_dictionary[is_brave].get(character)
    is_clever_fact = fact_dictionary[is_clever].get(character)
    does_fight_evil_fact = fact_dictionary[does_fight_evil].get(character)

    if(does_fight_evil_fact is False) : return False
    
    if ((has_wizard_parents_fact is True and is_clever_fact is True) or (has_wizard_parents_fact is True and is_brave_fact is True)) \
            or (is_clever_fact is True and is_brave_fact is True) :
        return True
    else:
        return False

# Facts
fact_dictionary: dict[Callable, dict[Character, bool | None]] = {
    has_wizard_parents: {
        harry: True,
        hermione: False,
        ron: True,
        draco: True,
        voldemort: True
    },
    is_brave: {},
    is_clever: {},
    does_fight_evil: {}
}

if __name__ == '__main__':
    for character in characters:
        print(f'Is {character.name} brave?')
        x = input()
        if x == "yes":
            fact_dictionary[is_brave][character] = True
        elif x == "no":
            fact_dictionary[is_brave][character] = False
        else:
            fact_dictionary[is_brave][character] = None

        print(f'Is {character.name} clever?')
        x = input()
        if x == "yes":
            fact_dictionary[is_clever][character] = True
        elif x == "no":
            fact_dictionary[is_clever][character] = False
        else:
            fact_dictionary[is_clever][character] = None

        print(f'Does {character.name} fight evil?')
        x = input()
        if x == "yes":
            fact_dictionary[does_fight_evil][character] = True
        elif x == "no":
            fact_dictionary[does_fight_evil][character] = False
        else:
            fact_dictionary[does_fight_evil][character] = None

        is_good = is_good_wizard(character)
        if is_good:
            print(f"{character.name} is a good wizard!")
        else:
            print(f"{character.name} is not a good wizard.")
