# Hotel in depth documentation

This should cover everything you need to understand how the project works and what was the design behind it.

TLDR:
`IBasicService` + `BasicService` is being used as a Ground-Base for the Services. They contain shared methods and contains `db context`.
Each service (for example: `BookingService`) uses `BasicService` class and implements its own intefrace (like `IBookingService`) that comes from `IBasicService`.
There we hold all the logic and handling of data from and into `db context`.
On top of them we have Controllers. Each comes from `GenericController` where all of the shared CRUD operations are. It keeps the code DRY but at the cost of harder readbility, it's up to discussion on
what do we want to focus on - depends on the team.
We inject our Services in the `Starup.cs` as it also allows us to run a db check before launching a project (logic is in `program.cs`).
On top of that we are introducing our own `LoggingServiceProvider` with `LoggingService` so we can gather logs and save them into `db context` easily.

As we aim to do TDD Unit Tests should cover most of the stuff that we can use with logic. Integration Tests checks if we can hit our `EndPoints`.

One of the logic points in the app is that each room that is not set as `Free` is treated as `Not available` and we work around that principle.
So if we manually set it as `During Cleaning` we have to specify why as it will be otherwise marked by checking-out.
We keep the data so instead of hard deleting we mark it with `IsDeleted` instead. In case we made a mistake it's easy to revert.

## Booking - api/Booking 

#### [GET] Get all Bookings

It will only show bookings that are marked as `not deleted`

#### [PUT] Book a room - /book/{roomId}

Accepts a `User` input from body - it checks the database if one exists with the same details (it checks if all fields are matching).
If it does not exist it will create a new one. It will change Room.RoomStatus From `Free` to `Occupied` 

#### [PUT] Check-out api/Booking/check-out/{roomId}

It gets the room that is currently not available and sets it status to `During Cleaning`. From this point on we need to use `RoomManagement` to change its fate.

## RoomManagement - api/RoomManagement

#### [GET] Get all rooms

Allows us to get all rooms with optional filters for `name`, `size` and `availability` - it only show rooms that are marked as `not deleted`.

#### [PUT] Mark as Free - /markAsFree/{roomId}

Sets selected room as `Free` with optional reason and optional details

#### [PUT] Mark as Occupied - /markAsOccupied/{roomId}

Sets selected room as `Occupied` with mandatory reason and optional details

#### [PUT] Mark as To Be Cleaned - /markForCleaning/{roomId}

Sets selected room as `During Cleaning` - with mandatory reason and optional details

#### [PUT] Mark as To Be Maintenance - /markForMaintenance/{roomId}

Sets selected room as `During Maintenance` - with mandatory reason and optional details

#### [PUT] Manually Lock - /manuallyLock{roomId}

Set's selected room `Manually Locked` - with mandatory reason and optional details


## Shared

As said before - to keep the code DRY we moved CRUD operations to shared controller. It's pretty self explanatory.

#### [GET] get by {id}

#### [POST] add new {entity}

#### [PUT] update {entity}

#### [DELETE] delete {id}

## Database

#### [Booking]

#### [PaymentMethod]

#### [Room]

#### [RoomSize]

#### [RoomStatus]

#### [RoomStatusDetails]

#### [RoomStatusDetailsPair]

#### [User]
