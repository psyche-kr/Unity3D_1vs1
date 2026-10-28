# Boss Battle

Low-poly 3D 그래픽의 액션 보스전 프로토타입입니다. 플레이어가 보스, 그리고 보스가 소환하는 해골과 싸웁니다. 2주 동안 혼자 기획부터 구현까지 만들었습니다.

[플레이 영상](https://youtu.be/6Mqn7DBhHJM)

| 항목 | 내용 |
| --- | --- |
| 기간 | 2024.10.21 – 11.04 (2주) |
| 인원 | 1명 |
| 엔진 | Unity 6000.0.24f1, C# |
| 담당 | 보스·몬스터 AI, 전투 시스템, 애니메이션·이펙트, UI 전부 |

## 구현 내용

### 보스 AI: 코루틴 기반 FSM

보스는 휴식(Rest), 판단(Think), 이동(Walk), 스펠(Spell) 네 상태로 움직입니다.

- 코루틴 하나가 현재 상태의 코루틴을 실행하고, 끝나면 다음 상태로 넘어갑니다. "애니메이션 재생, 2초 대기, 다음 상태 지정" 같은 시간 흐름을 코드 순서대로 쓸 수 있어서 이 방식을 골랐습니다.
- 판단 상태에서 플레이어와의 거리가 25를 넘으면 이동, 아니면 스펠로 갑니다. 이동 상태에서는 `NavMeshAgent`로 접근하다가 20 안으로 들어오면 다시 판단합니다.
- 스펠은 두 종류 중 무작위입니다. 하나는 `NavMeshAgent`로 플레이어를 계속 따라가는 유도 미사일(5초 뒤 소멸, 명중 시 피해)이고, 다른 하나는 해골 2마리 소환입니다.
- 보스와 해골은 공통 부모 `BaseMonster`(체력, 피격, 사망 처리)를 상속합니다. `DoRest`, `DoThink`, `DoWalk`, `DoSpell`, `DoAttack`을 가상 메서드로 두어 몬스터마다 필요한 것만 구현합니다.

### 해골 AI

소환(Spawn), 대기(Idle), 이동(Walking), 공격(Attack) 네 상태입니다. 감지 반경 10, 공격 반경 5를 기준으로 전이하고, 공격 전에 플레이어를 바라봅니다.

### 전투와 UI

- 일반 공격은 피해 1, 스킬은 피해 5입니다. 공격 판정은 태그(`Attack`, `Skill`)로 구분합니다. 일반 공격이 맞으면 스킬 스택이 쌓이고(최대 5), 스택에 따라 스킬 UI 이미지가 바뀝니다.
- 보스 체력바는 `Lerp`로 부드럽게 줄어듭니다. 피격하면 0.5초 동안 몬스터 색을 바꿔 맞았다는 것을 보여 줍니다.
- 공격 이펙트는 Trail Renderer로 만들었고, 승리와 패배 화면 UI도 구현했습니다.

## 스크립트

스크립트는 `Assets/04. Scripts`에 있습니다.

| 파일 | 역할 |
| --- | --- |
| `BaseMonster.cs` | 몬스터 공통 부모 (체력, 피격, 사망, 가상 메서드) |
| `BossMonster.cs`, `BossMissile.cs` | 보스 FSM, 유도 미사일 |
| `SkeletonMonster.cs`, `SkeletonAttack.cs` | 해골 FSM, 공격 |
| `PlayerController.cs`, `PlayerAttack.cs`, `PlayerStatus.cs`, `PlayerAnimation.cs` | 플레이어 이동·공격·상태·애니메이션 |
| `GameManager.cs` | 게임 진행(승리 처리 등) |

## 참고

- `Assets/02. Assets`의 이펙트, UI, 환경 에셋은 외부 에셋입니다.
- 이 프로젝트에서 만든 상태 머신 설계는 이후 팀 프로젝트 [The Fool](https://github.com/psyche-kr/Unity3D_The-Fool)의 몬스터 AI로 이어졌습니다.
