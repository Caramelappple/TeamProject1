/*
 * 스킬 추가하는 법
 * 
 * 1. KDH_SkillData로 ScriptableObject를 만들세요
 * 
 * 2. 방금 만든 SO(ScriptableObject)의 양식을 잘 적어주세요 (형식에 필요한 것들은 TestSkill들 있으니깐 그거 따라서 만들세요)
 * 
 * 3. 하이어라키에 Skill Selection Manager라고 있는데 거기에 만든 SO를 넣어줘요. (공간이 부족하면 늘리세요)
 * 
 * 4. Skill Selection Manager에 있는 All Skills의 개수가 SkillSystem의 Skills의 개수랑 똑같게 만들어주세요(!!! 늘리기만 하세요 Skills의 배열에 값을 넣으면 오류가 생김)
 */